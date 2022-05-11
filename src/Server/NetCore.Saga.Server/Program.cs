using Kaytune.Saga.Server;
using Kaytune.Saga.Server.InMemory;
using Kaytune.Saga.Server.Job;
using Kaytune.Saga.Server.Services;
using Microsoft.EntityFrameworkCore;
using NetCore.Saga.Abstract;
using NetCore.Saga.Abstract.Entity;
using NetCore.Saga.Abstract.Repository;
using NetCore.Saga.Server.Mysql;
using NetCore.Saga.Server.Mysql.Context;
using Quartz;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.UseMySql();
builder.Services.AddScoped<SecondCompensationJob>();
builder.Services.AddScoped<MinuteCompensationJob>();
builder.Services.AddScoped<LoadCompensationJob>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
ServiceLoader.SetServiceLoader(builder.Services.BuildServiceProvider()); 


var app = builder.Build();
app.InitDb();
await StartJobExtension.StartJob(builder.Services.BuildServiceProvider());
// Configure the HTTP request pipeline.
app.MapGrpcService<SagaEventService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();

