﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;
using ServiceStack.Text;
using ServiceStack.Web;

namespace ServiceStack.Admin;

[Icon(Svg = SvgIcons.Logs)]
[DataContract, ExcludeMetadata, Tag(TagNames.Admin)]
public class RequestLogs : IGet, IReturn<RequestLogsResponse>
{
    [DataMember(Order=1)] public int? BeforeSecs { get; set; }
    [DataMember(Order=2)] public int? AfterSecs { get; set; }
    [DataMember(Order=3)] public string OperationName { get; set; }
    [DataMember(Order=4)] public string IpAddress { get; set; }
    [DataMember(Order=5)] public string ForwardedFor { get; set; }
    [DataMember(Order=6)] public string UserAuthId { get; set; }
    [DataMember(Order=7)] public string SessionId { get; set; }
    [DataMember(Order=8)] public string Referer { get; set; }
    [DataMember(Order=9)] public string PathInfo { get; set; }
    [DataMember(Order=10)] public long[] Ids { get; set; }
    [DataMember(Order=11)] public int? BeforeId { get; set; }
    [DataMember(Order=12)] public int? AfterId { get; set; }
    [DataMember(Order=13)] public bool? HasResponse { get; set; }
    [DataMember(Order=14)] public bool? WithErrors { get; set; }
    [DataMember(Order=15)] public bool? EnableSessionTracking { get; set; }
    [DataMember(Order=16)] public bool? EnableResponseTracking { get; set; }
    [DataMember(Order=17)] public bool? EnableErrorTracking { get; set; }
    [DataMember(Order=18)] public TimeSpan? DurationLongerThan { get; set; }
    [DataMember(Order=19)] public TimeSpan? DurationLessThan { get; set; }
    [DataMember(Order=20)] public int Skip { get; set; }
    [DataMember(Order=21)] public int? Take { get; set; }
    [DataMember(Order=22)] public string OrderBy { get; set; }
}

[DataContract]
public class RequestLogsResponse
{
    [DataMember(Order=1)] public List<RequestLogEntry> Results { get; set; } = [];
    [DataMember(Order=2)] public Dictionary<string, string> Usage { get; set; }
    [DataMember(Order=3)] public int Total { get; set; }
    [DataMember(Order=4)] public ResponseStatus ResponseStatus { get; set; }
}

[DataContract]
public class GetAnalyticsReports : IGet, IReturn<AnalyticsReports>
{
    [DataMember(Order=1)] 
    public DateTime? Month { get; set; }
}

public enum AnalyticsType
{
    User,
    Day,
    ApiKey,
    IpAddress,
}

[DataContract]
public class GetApiAnalytics : IGet, IReturn<GetApiAnalyticsResponse>
{
    [DataMember(Order=1)]
    public DateTime? Month { get; set; }
    [DataMember(Order=2)]
    public AnalyticsType? Type { get; set; }
    [DataMember(Order=3)]
    public string Value { get; set; }
}

[DataContract]
public class GetApiAnalyticsResponse
{
    [DataMember(Order=1)]
    public Dictionary<string, long> Results { get; set; } = new();
}

[DataContract]
public class AnalyticsReports
{
    [DataMember(Order=1)] public Dictionary<string, RequestSummary> Apis { get; set; } = new();
    [DataMember(Order=2)] public Dictionary<string, RequestSummary> Users { get; set; } = new();
    [DataMember(Order=3)] public Dictionary<string, RequestSummary> Tags { get; set; } = new();
    [DataMember(Order=4)] public Dictionary<string, RequestSummary> Status { get; set; } = new();
    [DataMember(Order=5)] public Dictionary<string, RequestSummary> Days { get; set; } = new();
    [DataMember(Order=6)] public Dictionary<string, RequestSummary> ApiKeys { get; set; } = new();
    [DataMember(Order=7)] public Dictionary<string, RequestSummary> IpAddresses { get; set; } = new();
    [DataMember(Order=8)] public Dictionary<string, long> DurationRange { get; set; } = new();
}

[DataContract]
public class RequestSummary
{
    // op,user,tag,status,day,apikey,time(ms 0-50,51-100,101-200ms,1-2s,2s-5s,5s+)
    // public string Type { get; set; }
    [DataMember(Order=1)] public string Name { get; set; }
    [DataMember(Order=2)] public long Requests { get; set; }
    [DataMember(Order=3)] public long RequestLength { get; set; }
    [DataMember(Order=4)] public double Duration { get; set; }
    [DataMember(Order=5)] public Dictionary<int,long> Status { get; set; }
}

[DefaultRequest(typeof(RequestLogs))]
public class RequestLogsService(IRequestLogger requestLogger) : Service
{
    private static readonly Dictionary<string, string> Usage = new() {
        {"int BeforeSecs",              "Requests before elapsed time"},
        {"int AfterSecs",               "Requests after elapsed time"},
        {"string IpAddress",            "Requests matching Ip Address"},
        {"string ForwardedFor",         "Requests matching Forwarded Ip Address"},
        {"string UserAuthId",           "Requests matching UserAuthId"},
        {"string SessionId",            "Requests matching SessionId"},
        {"string Referer",              "Requests matching Http Referer"},
        {"string PathInfo",             "Requests matching PathInfo"},
        {"int BeforeId",                "Requests before RequestLog Id"},
        {"int AfterId",                 "Requests after RequestLog Id"},
        {"bool WithErrors",             "Requests with errors"},
        {"bool EnableSessionTracking",  "Turn On/Off Session Tracking"},
        {"bool EnableResponseTracking", "Turn On/Off Tracking of Responses"},
        {"bool EnableErrorTracking",    "Turn On/Off Tracking of Errors"},
        {"TimeSpan DurationLongerThan", "Requests with a duration longer than"},
        {"TimeSpan DurationLessThan",   "Requests with a duration less than"},
        {"int Skip",                    "Skip past N results"},
        {"int Take",                    "Only look at last N results"},
        {"string OrderBy",              "Order results by specified fields, e.g. SessionId,-Id"},
    };

    public async Task<object> Any(RequestLogs request)
    {
        var feature = AssertPlugin<RequestLogsFeature>();
        if (!HostContext.DebugMode)
            await RequiredRoleAttribute.AssertRequiredRoleAsync(Request, feature.AccessRole);

        if (request.EnableSessionTracking.HasValue)
            requestLogger.EnableSessionTracking = request.EnableSessionTracking.Value;

        var defaultLimit = feature?.DefaultLimit ?? 100;

        var now = DateTime.UtcNow;
        var snapshot = requestLogger.GetLatestLogs(null);
        var logs = snapshot.AsQueryable();

        if (request.BeforeSecs.HasValue)
            logs = logs.Where(x => (now - x.DateTime) <= TimeSpan.FromSeconds(request.BeforeSecs.Value));
        if (request.AfterSecs.HasValue)
            logs = logs.Where(x => (now - x.DateTime) > TimeSpan.FromSeconds(request.AfterSecs.Value));
        if (!request.OperationName.IsNullOrEmpty())
            logs = logs.Where(x => x.OperationName == request.OperationName);
        if (!request.IpAddress.IsNullOrEmpty())
            logs = logs.Where(x => x.IpAddress == request.IpAddress);
        if (!request.ForwardedFor.IsNullOrEmpty())
            logs = logs.Where(x => x.ForwardedFor == request.ForwardedFor);
        if (!request.UserAuthId.IsNullOrEmpty())
            logs = logs.Where(x => x.UserAuthId == request.UserAuthId);
        if (!request.SessionId.IsNullOrEmpty())
            logs = logs.Where(x => x.SessionId == request.SessionId);
        if (!request.Referer.IsNullOrEmpty())
            logs = logs.Where(x => x.Referer == request.Referer);
        if (!request.PathInfo.IsNullOrEmpty())
            logs = logs.Where(x => x.PathInfo == request.PathInfo);
        if (!request.Ids.IsEmpty())
            logs = logs.Where(x => request.Ids.Contains(x.Id));
        if (request.BeforeId.HasValue)
            logs = logs.Where(x => x.Id <= request.BeforeId);
        if (request.AfterId.HasValue)
            logs = logs.Where(x => x.Id > request.AfterId);
        if (request.WithErrors.HasValue)
            logs = request.WithErrors.Value
                ? logs.Where(x => x.ErrorResponse != null || x.StatusCode >= 400)
                : logs.Where(x => x.ErrorResponse == null);
        if (request.DurationLongerThan.HasValue)
            logs = logs.Where(x => x.RequestDuration > request.DurationLongerThan.Value);
        if (request.DurationLessThan.HasValue)
            logs = logs.Where(x => x.RequestDuration < request.DurationLessThan.Value);

        var query = string.IsNullOrEmpty(request.OrderBy)
            ? logs.OrderByDescending(x => x.Id)
            : logs.OrderBy(request.OrderBy);

        var results = query.Skip(request.Skip);
        results = results.Take(request.Take.GetValueOrDefault(defaultLimit));

        return new RequestLogsResponse {
            Results = results.ToList(),
            Total = snapshot.Count,
            Usage = Usage,
        };
    }

    public async Task<object> Any(GetAnalyticsReports request)
    {
        var feature = AssertPlugin<RequestLogsFeature>();
        if (!HostContext.DebugMode)
            await RequiredRoleAttribute.AssertRequiredRoleAsync(Request, feature.AccessRole);

        if (feature.RequestLogger is not IRequireAnalytics analytics)
            throw new NotSupportedException(feature.RequestLogger + " does not support IRequireAnalytics");

        var ret = analytics.GetAnalyticsReports(request.Month ?? DateTime.UtcNow);

        var userResolver = Request?.TryResolve<IUserResolver>();
        if (userResolver != null)
        {
            var allUserIds = ret.Users.Where(x => x.Value.Name == null)
                .Map(x => x.Key);
            var allUsers = await userResolver.GetUsersByIdsAsync(Request, allUserIds).ConfigAwait();
            var allUsersMap = new Dictionary<string, string>();
            foreach (var user in allUsers)
            {
                if (user.TryGetValue(nameof(IUserAuth.Id), out var oId)
                    && user.TryGetValue(nameof(IUserAuth.UserName), out var oUserName))
                {
                    allUsersMap[oId.ToString()!] = oUserName.ToString();
                }
            }
            foreach (var user in ret.Users)
            {
                if (user.Value.Name == null && allUsersMap.TryGetValue(user.Key.ToString()!, out var userName))
                {
                    user.Value.Name = userName;
                }
            }
        }

        return ret;
    }

    public async Task<object> Any(GetApiAnalytics request)
    {
        var feature = AssertPlugin<RequestLogsFeature>();
        if (!HostContext.DebugMode)
            await RequiredRoleAttribute.AssertRequiredRoleAsync(Request, feature.AccessRole);

        if (feature.RequestLogger is not IRequireAnalytics analytics)
            throw new NotSupportedException(feature.RequestLogger + " does not support IRequireAnalytics");
        
        if (request.Type == null)
            throw new ArgumentNullException(nameof(request.Type));
        if (request.Value == null)
            throw new ArgumentNullException(nameof(request.Value));
        
        var ret = analytics.GetApiAnalytics(request.Month ?? DateTime.UtcNow,
            request.Type.Value, request.Value);

        return new GetApiAnalyticsResponse
        {
            Results = ret
        };
    }
}