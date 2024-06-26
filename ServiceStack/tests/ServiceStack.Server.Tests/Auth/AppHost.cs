﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Funq;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using ServiceStack.Web;

#if NETCORE
using Microsoft.Extensions.DependencyInjection;
#endif

//The entire C# code for the stand-alone RazorRockstars demo.
namespace ServiceStack.Server.Tests.Auth
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost() : base("Test Auth", typeof(AppHost).Assembly) { }

        public RSAParameters? JwtRsaPrivateKey;
        public RSAParameters? JwtRsaPublicKey;
        public bool JwtEncryptPayload = false;
        public List<byte[]> FallbackAuthKeys = new List<byte[]>();
        public List<RSAParameters> FallbackPublicKeys = new List<RSAParameters>();
        public Func<IRequest, IAuthRepository> GetAuthRepositoryFn;

        public Action<Container> Use;

#if NETCORE
        public override void Configure(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddMvc();
        }
#endif

        public override void Configure(Container container)
        {
            Use?.Invoke(container);
            
            SetConfig(new HostConfig
            {
                AdminAuthSecret = "secret",
                DebugMode = true,
                WebHostPhysicalPath = Path.GetFullPath("../../../"),
            });

#if NETCORE
            Plugins.Add(new Mvc.RazorFormat());
#else
            Plugins.Add(new Razor.RazorFormat());
#endif

            var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            container.Register<IDbConnectionFactory>(dbFactory);

            dbFactory.RegisterConnection("testdb", MapProjectPath("~/App_Data/test.sqlite"), SqliteDialect.Provider);

            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<Rockstar>(); //Create table if not exists
                db.Insert(Rockstar.SeedData); //Populate with seed data
            }

            using (var db = dbFactory.OpenDbConnection("testdb"))
            {
                db.DropAndCreateTable<Rockstar>(); //Create table if not exists
                db.Insert(new Rockstar(1, "Test", "Database", 27));
            }

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                    new BasicAuthProvider(AppSettings),
                    new CredentialsAuthProvider(AppSettings),
                    new ApiKeyAuthProvider(AppSettings) { RequireSecureConnection = false },
                    new JwtAuthProvider(AppSettings)
                    {
                        AuthKey = JwtRsaPrivateKey != null || JwtRsaPublicKey != null ? null : AesUtils.CreateKey(),
                        RequireSecureConnection = false,
                        HashAlgorithm = JwtRsaPrivateKey != null || JwtRsaPublicKey != null ? "RS256" : "HS256",
                        PublicKey = JwtRsaPublicKey,
                        PrivateKey = JwtRsaPrivateKey,
                        EncryptPayload = JwtEncryptPayload,
                        FallbackAuthKeys = FallbackAuthKeys,
                        FallbackPublicKeys = FallbackPublicKeys,
                    },
                })
            {
                IncludeRegistrationService = true,
            });

            container.Resolve<IAuthRepository>().InitSchema();
        }

        public override IDbConnection GetDbConnection(IRequest req = null)
        {
            var apiKey = req.GetApiKey();
            return apiKey is { Environment: "test" }
                ? TryResolve<IDbConnectionFactory>().OpenDbConnection("testdb")
                : base.GetDbConnection(req);
        }

        public override IAuthRepository GetAuthRepository(IRequest req = null)
        {
            return GetAuthRepositoryFn != null
                ? GetAuthRepositoryFn(req)
                : base.GetAuthRepository(req);
        }
    }

    public class Rockstar
    {
        public static Rockstar[] SeedData = new[] {
            new Rockstar(1, "Jimi", "Hendrix", 27),
            new Rockstar(2, "Janis", "Joplin", 27),
            new Rockstar(3, "Jim", "Morrisson", 27),
            new Rockstar(4, "Kurt", "Cobain", 27),
            new Rockstar(5, "Elvis", "Presley", 42),
            new Rockstar(6, "Michael", "Jackson", 50),
        };

        [AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public bool Alive { get; set; }

        public Rockstar() { }
        public Rockstar(int id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
    }

    [Route("/rockstars")]
    [Route("/rockstars/aged/{Age}")]
    [Route("/rockstars/delete/{Delete}")]
    [Route("/rockstars/{Id}")]
    public class Rockstars : IReturn<RockstarsResponse>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public bool Alive { get; set; }
        public string Delete { get; set; }
        public string View { get; set; }
        public string Template { get; set; }
    }

    [DataContract] //Attrs for CSV Format to recognize it's a DTO and serialize the Enumerable property
    public class RockstarsResponse
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int? Aged { get; set; }
        [DataMember]
        public List<Rockstar> Results { get; set; }
    }

    [Route("/ilist1/{View}")]
    public class IList1
    {
        public string View { get; set; }
    }

    [Route("/ilist2/{View}")]
    public class IList2
    {
        public string View { get; set; }
    }

    [Route("/ilist3/{View}")]
    public class IList3
    {
        public string View { get; set; }
    }

    [Route("/partialmodel")]
    public class PartialModel
    {
        public IEnumerable<PartialChildModel> Items { get; set; }
    }
    public class PartialChildModel
    {
        public string SomeProperty { get; set; }
    }

    public class GetAllRockstars : IReturn<RockstarsResponse> { }

    [Authenticate]
    public class SecureServices : Service
    {
        public object Any(GetAllRockstars request)
        {
            return new RockstarsResponse { Results = Db.Select<Rockstar>() };
        }
    }

    public class RockstarsService : Service
    {
        public object Get(Rockstars request)
        {
            if (request.Delete == "reset")
            {
                Db.DeleteAll<Rockstar>();
                Db.Insert(Rockstar.SeedData);
            }
            else if (request.Delete.IsInt())
            {
                Db.DeleteById<Rockstar>(request.Delete.ToInt());
            }

            var response = new RockstarsResponse
            {
                Aged = request.Age,
                Total = Db.Scalar<int>("select count(*) from Rockstar"),
                Results = request.Id != default(int) ?
                    Db.Select<Rockstar>(q => q.Id == request.Id)
                      : request.Age.HasValue ?
                    Db.Select<Rockstar>(q => q.Age == request.Age.Value)
                      : Db.Select<Rockstar>()
            };

            if (request.View != null || request.Template != null)
                return new HttpResult(response)
                {
                    View = request.View,
                    Template = request.Template,
                };

            return response;
        }

        public object Post(Rockstars request)
        {
            Db.Insert(request.ConvertTo<Rockstar>());
            return Get(new Rockstars());
        }

        public IList<Rockstar> Get(IList1 request)
        {
            base.Request.Items["View"] = request.View;
            return Db.Select<Rockstar>();
        }

        public List<Rockstar> Get(IList2 request)
        {
            base.Request.Items["View"] = request.View;
            return Db.Select<Rockstar>();
        }

        public object Get(IList3 request)
        {
            base.Request.Items["View"] = request.View;
            return Db.Select<Rockstar>();
        }

        public PartialModel Any(PartialModel request)
        {
            return new PartialModel
            {
                Items = 5.Times(x => new PartialChildModel
                {
                    SomeProperty = "value " + x
                })
            };
        }

        public void Any(RedirectWithoutQueryString request) { }
    }

    public class RedirectWithoutQueryStringFilterAttribute : RequestFilterAttribute
    {
        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            if (req.QueryString.Count > 0)
            {
                res.RedirectToUrl(req.PathInfo);
            }
        }
    }

    [RedirectWithoutQueryStringFilter]
    public class RedirectWithoutQueryString
    {
        public int Id { get; set; }
    }

    [Route("/Content/hello/{Name*}")]
    public class TestWildcardRazorPage
    {
        public string Name { get; set; }
    }

    public class IssueServices : Service
    {
        public object Get(TestWildcardRazorPage request)
        {
            return request;
        }
    }

    [Route("/test/session")]
    public class TestSession : IReturn<TestSessionResponse> { }

    [Route("/test/session/view")]
    public class TestSessionView : IReturn<TestSessionResponse> { }

    public class TestSessionResponse
    {
        public string UserAuthId { get; set; }
        public bool IsAuthenticated { get; set; }
    }

    public class TestSessionAttribute : RequestFilterAttribute
    {
        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            var session = req.GetSession();
            if (!session.IsAuthenticated)
            {
                res.StatusCode = (int)HttpStatusCode.Unauthorized;
                res.EndRequestWithNoContent();
            }
        }
    }

    public class TestSessionService : Service
    {
        [TestSession]
        public object Any(TestSession request)
        {
            var session = base.Request.GetSession();
            return new TestSessionResponse
            {
                UserAuthId = session.UserAuthId,
                IsAuthenticated = session.IsAuthenticated,
            };
        }

        public object Any(TestSessionView request)
        {
            return new TestSessionResponse();
        }
    }
}
