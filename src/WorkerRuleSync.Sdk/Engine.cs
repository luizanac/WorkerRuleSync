namespace WorkerRuleSync.Sdk;

internal class Engine : IEngine
{
    private readonly RulesEngine.RulesEngine _engine;

    public Engine(RuleEngineConfig config, IWorkflowSyncService workflowSyncService)
    {
        _engine = new RulesEngine.RulesEngine(Array.Empty<Workflow>(), new ReSettings
        {
            CustomTypes = config.CustomTypes,
            CustomActions = config.CustomActions
        });

        workflowSyncService.OnUpdateWorkflows += workflows => _engine.AddOrUpdateWorkflow(workflows);
    }

    public async Task<RuleResultTree> ExecuteDecisionTableAsync(string workflow, params RuleParameter[] parameters)
    {
        var result = await _engine.ExecuteAllRulesAsync(workflow, parameters);

        if (result.Count(x => x.IsSuccess) > 1)
            throw new InvalidOperationException("Decision tables podem ter somente uma regra avaliada com sucesso!");

        return result.Single(x => x.IsSuccess);
    }
}