using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WorkerRuleSync.Sdk;
using WorkerRuleSync.Server.Hubs;

namespace WorkerRuleSync.Server.Controllers;

[ApiController]
[Route("workflows")]
public class WorkflowController : ControllerBase
{
    private readonly IHubContext<WorkflowHub> _hub;
    private static readonly IList<WorkflowVersion> WorkflowVersions = new List<WorkflowVersion>();

    private const string WorkerAJson = """
{
    "workflow": {
        "WorkflowName": "WorkerA",
        "Rules": [
        {
            "RuleName": "GiveDiscount30",
            "SuccessEvent": "30",
            "Expression": "1 == 1"
        },
        {
            "RuleName": "GiveDiscount50",
            "SuccessEvent": "50",
            "Expression": "1 != 1"
        }
        ]
    },
    "version": 1
}
""";

    private const string WorkerBJson = """
{
    "workflow": {
        "WorkflowName": "WorkerB",
        "Rules": [
        {
            "RuleName": "GiveDiscount30",
            "SuccessEvent": "30",
            "Expression": "1 == 1"
        },
        {
            "RuleName": "GiveDiscount50",
            "SuccessEvent": "50",
            "Expression": "1 != 1"
        }
        ]
    },
    "version": 1
}
""";

    public WorkflowController(IHubContext<WorkflowHub> hub)
    {
        _hub = hub;
    }

    [HttpGet]
    public IActionResult GetAll(string workflowNames)
    {
        if (!WorkflowVersions.Any())
        {
            WorkflowVersions.Add(JsonSerializer.Deserialize<WorkflowVersion>(WorkerAJson)!);
            WorkflowVersions.Add(JsonSerializer.Deserialize<WorkflowVersion>(WorkerBJson)!);
        }

        var workflows =
            workflowNames.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return Ok(WorkflowVersions.Where(x => workflows.Contains(x.Workflow.WorkflowName)));
    }

    //http://yarp.test:5282/server/swagger/index.html
    
    [HttpPost]
    public IActionResult Create(WorkflowVersion workflowVersion)
    {
        var workflow =
            WorkflowVersions.FirstOrDefault(x =>
                x.Workflow.WorkflowName.Equals(workflowVersion.Workflow.WorkflowName) &&
                x.Version < workflowVersion.Version);

        if (workflow is not null)
            WorkflowVersions.Remove(workflow);

        WorkflowVersions.Add(workflowVersion);
        _hub.Clients.All.SendAsync($"{workflowVersion.Workflow.WorkflowName}-updated", workflowVersion);

        return Ok(workflowVersion);
    }
}