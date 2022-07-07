using System.Collections.Generic;
using ServiceStack.Messaging;
using ServiceStack.Redis;

namespace ServiceStack;

public class AdminDashboard : IReturn<AdminDashboardResponse> {}
public class AdminDashboardResponse : IHasResponseStatus
{
    public ServerStats ServerStats { get; set; }
    public ResponseStatus ResponseStatus { get; set; }
}

public class ServerStats
{
    public Dictionary<string, long> Redis { get; set; }
    public Dictionary<string, string> ServerEvents { get; set; }
    public string MqDescription { get; set; }
    public Dictionary<string, long> MqWorkers { get; set; }
}


[DefaultRequest(typeof(AdminDashboard))]
[Restrict(VisibilityTo = RequestAttributes.Localhost)]
public class AdminDashboardServices : Service
{
    public object Any(AdminDashboard request)
    {
        var mqServer = TryResolve<IMessageService>();
        var mqWorker = mqServer?.GetStats();
        var stats = new ServerStats {
            Redis = (TryResolve<IRedisClientsManager>() as IHasStats)?.Stats,
            ServerEvents = TryResolve<IServerEvents>()?.GetStats(),
            MqDescription = mqServer?.GetStatsDescription(),
            MqWorkers = mqWorker.ToDictionary(),
        };
        
        return new AdminDashboardResponse
        {
            ServerStats = stats,
        };
    }
}

public static class AdminStatsUtils
{
    public static Dictionary<string, long> ToDictionary(this IMessageHandlerStats stats)
    {
        if (stats == null)
            return null;
        
        var to = new Dictionary<string, long>
        {
            [nameof(stats.TotalMessagesProcessed)] = stats.TotalMessagesProcessed,
            [nameof(stats.TotalMessagesFailed)] = stats.TotalMessagesFailed,
            [nameof(stats.TotalRetries)] = stats.TotalRetries,
            [nameof(stats.TotalNormalMessagesReceived)] = stats.TotalNormalMessagesReceived,
            [nameof(stats.TotalPriorityMessagesReceived)] = stats.TotalPriorityMessagesReceived,
        };
        
        return to;
    }
}