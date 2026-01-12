using Hangfire;
using NetBlaze.Api.Extensions;
using NetBlaze.Application.Jobs.RandomCheck;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();


var app = builder.Build();


app.ConsumeServices();

RecurringJob.AddOrUpdate<RandomCheckJob>(
    "random-check-daily-generator",
    job => job.GenerateTodaySchedules(),
    Cron.Daily);

await app.RunAsync();
