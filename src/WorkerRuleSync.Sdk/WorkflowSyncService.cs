using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace WorkerRuleSync.Sdk;

internal class WorkflowSyncService : IWorkflowSyncService
{
    public Action<Workflow[]>? OnUpdateWorkflows { get; set; }

    private readonly RuleEngineConfig _config;
    private readonly HubConnection _connection;
    private readonly HttpClient _httpClient;

    public WorkflowSyncService(RuleEngineConfig config)
    {
        _config = config;

        _httpClient = new HttpClient { BaseAddress = new Uri(_config.RuleServerHost) };

        _connection = new HubConnectionBuilder()
            .WithUrl($"{_config.RuleServerHost}/server/workflowHub")
            .ConfigureLogging(x => x.SetMinimumLevel(LogLevel.Information))
            .WithAutomaticReconnect(new RetryPolicy())
            .Build();

        Configure();
    }

    private void Configure()
    {
        _connection.Reconnected += s =>
        {
            Console.WriteLine($"Recconected: {s}");
            return Task.FromResult(0);
        };

        _connection.Reconnecting += error =>
        {
            Console.WriteLine($"Reconnecting: {error?.Message}");
            return Task.FromResult(0);
        };

        foreach (var configWorkflow in _config.Workflows)
        {
            _connection.On($"{configWorkflow}-updated",
                (WorkflowVersion workflowVersion) =>
                {
                    OnUpdateWorkflows?.Invoke(new[] { workflowVersion.Workflow });
                });
        }
    }

    private async Task FetchWorkflows()
    {
        var path = $"/server/workflows?workflowNames={string.Join(',', _config.Workflows)}";
        var workflowsVersions = await _httpClient.GetFromJsonAsync<WorkflowVersion[]>(path);
        if (workflowsVersions is null || workflowsVersions.Length == 0)
            throw new InvalidOperationException("invalid workflows versions specified");

        OnUpdateWorkflows?.Invoke(workflowsVersions.Select(x => x.Workflow).ToArray());
    }

    public async Task Start()
    {
        await FetchWorkflows();
        await _connection.StartAsync();
    }
}