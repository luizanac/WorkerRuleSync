namespace WorkerRuleSync.Sdk.Abstractions;

public interface IEngine
{
    Task<RuleResultTree> ExecuteDecisionTableAsync(string workflow, params RuleParameter[] parameters);
}