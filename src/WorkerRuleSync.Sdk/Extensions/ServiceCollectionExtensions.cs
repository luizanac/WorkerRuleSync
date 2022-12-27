namespace WorkerRuleSync.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRuleEngine(this IServiceCollection services, RuleEngineConfig config)
    {
        var ruleSyncService = new WorkflowSyncService(config);
        services.AddSingleton<IEngine>(new Engine(config, ruleSyncService));
        ruleSyncService.Start().ConfigureAwait(false).GetAwaiter().GetResult();
        return services;
    }
}