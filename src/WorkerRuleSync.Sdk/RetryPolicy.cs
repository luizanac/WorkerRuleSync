using Microsoft.AspNetCore.SignalR.Client;

namespace WorkerRuleSync.Sdk;

public class RetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext) => TimeSpan.FromSeconds(5);
}