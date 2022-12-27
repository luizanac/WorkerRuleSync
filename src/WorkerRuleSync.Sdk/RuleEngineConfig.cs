namespace WorkerRuleSync.Sdk;

public class RuleEngineConfig
{
    public required string RuleServerHost { get; set; }
    public required string[] Workflows { get; set; }

    /// <summary>
    /// Get/Set the custom types to be used in Rule expressions
    /// </summary>
    public Type[]? CustomTypes { get; set; }

    /// <summary>
    /// Get/Set the custom actions that can be used in the Rules
    /// </summary>
    public Dictionary<string, Func<ActionBase>>? CustomActions { get; set; }

    public int ReconnectAttempInterval { get; set; } = 5000;
}