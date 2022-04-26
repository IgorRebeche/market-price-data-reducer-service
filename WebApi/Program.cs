using Application.Extensions;
using Infrastructure;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try 
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddOptions();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Masstransit
    builder.Services.AddMassTransit(x =>
    {
         x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetSection("RabbitMqConfigurationOptions:Host").Value, "/", h =>
            {
                h.Username(builder.Configuration.GetSection("RabbitMqConfigurationOptions:Username").Value);
                h.Password(builder.Configuration.GetSection("RabbitMqConfigurationOptions:Password").Value);
            });
         });
    });
    builder.Services.AddMassTransitHostedService();

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Development mode enabled!");
    }
    
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Starting Application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
