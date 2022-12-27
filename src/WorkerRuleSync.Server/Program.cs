using Microsoft.OpenApi.Models;
using WorkerRuleSync.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
        {
            if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host")) return;
            if (!httpRequest.Headers.ContainsKey("X-Server-Base-Path")) return;
            
            var serverUrl = $"{httpRequest.Scheme}://{httpRequest.Headers["X-Forwarded-Host"]}/{httpRequest.Headers["X-Server-Base-Path"]}";
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
        });
    });
    app.UseSwaggerUI();
}

app.MapHub<WorkflowHub>("/workflowHub");
app.MapControllers();

app.Run();