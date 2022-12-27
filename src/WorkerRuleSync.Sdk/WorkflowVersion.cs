using System.Text.Json.Serialization;

namespace WorkerRuleSync.Sdk;

public class WorkflowVersion
{
    [JsonPropertyName("workflow")]
    public required Workflow Workflow { get; set; }
    
    [JsonPropertyName("version")]
    public int Version { get; set; }
}