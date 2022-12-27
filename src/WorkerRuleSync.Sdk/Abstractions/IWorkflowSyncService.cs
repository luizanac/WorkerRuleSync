namespace WorkerRuleSync.Sdk.Abstractions;

internal interface IWorkflowSyncService
{
    public Task Start();
    public Action<Workflow[]>? OnUpdateWorkflows { get; set; }
}