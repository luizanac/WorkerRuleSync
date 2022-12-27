using WorkerRuleSync.Sdk;
using WorkerRuleSync.Sdk.Extensions;
using WorkerRuleSync.WorkerB;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddRuleEngine(new RuleEngineConfig
        {
            RuleServerHost = "http://yarp",
            Workflows = new[] { "WorkerB" }
        });
        
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
