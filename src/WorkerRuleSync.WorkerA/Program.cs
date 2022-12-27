using WorkerRuleSync.Sdk;
using WorkerRuleSync.Sdk.Extensions;
using WorkerRuleSync.WorkerA;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddRuleEngine(new RuleEngineConfig
        {
            RuleServerHost = "http://yarp",
            Workflows = new[] { "WorkerA" }
        });

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();