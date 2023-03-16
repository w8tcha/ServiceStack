﻿#nullable enable
#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ServiceStack.Host;
using ServiceStack.IO;

namespace ServiceStack.Mvc;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RenderStaticAttribute : Attribute
{
    public string? Path { get; }
    public RenderStaticAttribute(){}
    public RenderStaticAttribute(string path)
    {
        Path = path;
    }
}

public class RenderContext
{
    public IServiceProvider Services { get; }
    public IVirtualFile RazorFile { get; }
    public RenderContext(IServiceProvider services, IVirtualFile razorFile)
    {
        Services = services;
        RazorFile = razorFile;
    }

    public T Resolve<T>() => Services.Resolve<T>();
    public T? TryResolve<T>() => Services.TryResolve<T>();
}
public interface IRenderStatic {}
public interface IRenderStatic<T> : IRenderStatic where T : PageModel
{
    List<T> GetStaticProps(RenderContext ctx);
}

public interface IRenderStaticWithPath<T> : IRenderStatic<T> where T : PageModel
{
    string? GetStaticPath(T model);
}


public class RazorSsg
{
    public static async Task<string?> GetPageRouteAsync(IVirtualFile razorFile)
    {
        using var readFs = razorFile.OpenText();
        var firstLine = await readFs.ReadLineAsync();
        if (firstLine?.StartsWith("@page") != true) return null;
        var pageRoute = firstLine["@page".Length..].Trim().StripQuotes();
        if (!string.IsNullOrEmpty(pageRoute))
            return pageRoute;
        return null;
    }

    public static HttpContext CreateHttpContext(ServiceStackHost appHost, string pathInfo)
    {
        var url = "https://localhost:5001".CombineWith(pathInfo);
        var ctx = new DefaultHttpContext
        {
            RequestServices = appHost.Container,
            Items = {
                [Keywords.IRequest] = new BasicHttpRequest(null,
                    RequestAttributes.LocalSubnet | RequestAttributes.Http | RequestAttributes.InProcess)
                {
                    PathInfo = pathInfo,
                    AbsoluteUri = url,
                    RawUrl = url,
                }
            }
        };
        return ctx;
    }
    
    public static async Task PrerenderAsync(ServiceStackHost appHost, IEnumerable<IVirtualFile> razorFiles, string distDir)
    {
        var log = appHost.Resolve<ILogger<RazorSsg>>();
        
        var razorPages = appHost.Resolve<RazorPagesEngine>();
        foreach (var razorFile in razorFiles)
        {
            var isMainPage = razorFile.VirtualPath.EndsWith("Layout.cshtml");
            var viewResult = razorPages.GetView(razorFile.VirtualPath, isMainPage: isMainPage);
            if (!viewResult.Success) continue;
            
            var razorPage = (viewResult.View as RazorView)?.RazorPage;
            if (razorPage == null) continue;
            
            var razorPageType = razorPage.GetType();
            var attrs = razorPageType.AllAttributes<RenderStaticAttribute>();
            foreach (var attr in attrs)
            {
                var pageRoute = await GetPageRouteAsync(razorFile);
                var staticPath = attr.Path ?? pageRoute;

                if (string.IsNullOrEmpty(staticPath))
                    throw new Exception($"Razor Page {razorFile.VirtualPath} contains an empty [RenderStatic] in @page with no route");

                if (staticPath.EndsWith("/"))
                    staticPath += "index.html";
                else if (staticPath.IndexOf('.') == -1)
                    staticPath += ".html";
                
                var toPath = distDir.CombineWith(staticPath);
                FileSystemVirtualFiles.AssertDirectory(Path.GetDirectoryName(toPath));
                
                log.LogInformation("Rendering {0} to {1}", razorFile.VirtualPath, staticPath);
                await using var fs = File.OpenWrite(toPath);
                var ctx = CreateHttpContext(appHost, pathInfo: pageRoute ?? staticPath.LastLeftPart('.'));
                await razorPages.WriteHtmlAsync(fs, viewResult.View, model:null, ctx:ctx);
            }

            if (razorPage is not IRenderStatic) 
                continue;

            var renderStaticDef = razorPageType.GetTypeWithGenericTypeDefinitionOf(typeof(IRenderStatic<>));
            if (renderStaticDef == null) continue;

            var modelType = renderStaticDef.GetGenericArguments()[0];
            var method = typeof(RazorSsg).GetMethod(nameof(RenderStaticRazorPageAsync));
            var genericMi = method.MakeGenericMethod(modelType);
            var task = (Task) genericMi.Invoke(null, new object[] { appHost, razorFile, distDir });
            await task;
        }
    }

    public static async Task RenderStaticRazorPageAsync<T>(ServiceStackHost appHost, IVirtualFile razorFile, string destDir) where T : PageModel
    {
        var log = appHost.Resolve<ILogger<RazorSsg>>();
        var razorPages = appHost.Resolve<RazorPagesEngine>();
        var viewResult = razorPages.GetView(razorFile.VirtualPath);
        if (!viewResult.Success)
            throw new Exception($"Could not resolve Razor Page at: {razorFile.VirtualPath}");
            
        var razorPage = (viewResult.View as RazorView)?.RazorPage;
        if (razorPage == null)
            throw new Exception($"Razor Page is not a RazorView: {razorFile.VirtualPath}");
        
        var pageRoute = await GetPageRouteAsync(razorFile); 
        
        var renderStatic = (IRenderStatic<T>)razorPage;
        var pageModels = renderStatic.GetStaticProps(new RenderContext(appHost.Container, razorFile));

        if (pageModels.Count > 0)
        {
            log.LogInformation("Rendering {0} {1}'s in {2}...", pageModels.Count, typeof(T).Name, razorFile.VirtualPath);            
        }
        for (var i = 0; i < pageModels.Count; i++)
        {
            var pageModel = pageModels[i];
            string? staticPath = null;
            if (razorPage is IRenderStaticWithPath<T> renderStaticWithPath)
            {
                staticPath = renderStaticWithPath.GetStaticPath(pageModel);
            }
            else if (pageRoute != null)
            {
                var restRoute = new RestRoute(typeof(T), pageRoute, ServiceStack.HttpMethods.Get, 0);
                var result = restRoute.Apply(pageModel, ServiceStack.HttpMethods.Get);
                if (result.Matches)
                    staticPath = result.Uri + ".html";
            }

            if (staticPath == null)
            {
                log.LogWarning("Could not resolve static path for {0}, ignoring...",
                    pageRoute ?? razorFile.VirtualPath);
                return;
            }

            viewResult = razorPages.GetView(razorFile.VirtualPath);
            if (!viewResult.Success)
                return;

            var toPath = destDir.CombineWith(staticPath);
            FileSystemVirtualFiles.AssertDirectory(Path.GetDirectoryName(toPath));

            log.LogInformation("Rendering {0}/{1} to {2}", i+1, pageModels.Count, staticPath);
            
            await using var fs = File.OpenWrite(toPath);
            var ctx = CreateHttpContext(appHost, pathInfo: pageRoute ?? staticPath.LastLeftPart('.'));
            await razorPages.WriteHtmlAsync(fs, viewResult.View, model: pageModel, ctx: ctx);
        }
    }
}

#endif