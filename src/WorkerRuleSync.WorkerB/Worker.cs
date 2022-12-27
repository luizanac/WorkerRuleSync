using RulesEngine.Models;
using WorkerRuleSync.Sdk.Abstractions;

namespace WorkerRuleSync.WorkerB;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEngine _engine;

    public Worker(ILogger<Worker> logger, IEngine engine)
    {
        _logger = logger;
        _engine = engine;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var input = new RuleParameter("input", "input");
            var result = await _engine.ExecuteDecisionTableAsync("WorkerB", input);
            _logger.LogInformation(result.Rule.SuccessEvent);
            await Task.Delay(1000, stoppingToken);
        }
    }
}