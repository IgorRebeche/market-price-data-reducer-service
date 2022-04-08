using Infrastructure;
using MassTransit;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try 
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddOptions();
    builder.Services.AddInfrastructure(builder.Configuration);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Masstransit
    // builder.Services.AddMassTransit(x =>
    // {
    //     //x.AddConsumer<TickerCollectedConsumer>();
    //     x.UsingRabbitMq((context, cfg) =>
    //     {
    //         cfg.Host("localhost", "/", h =>
    //         {
    //             h.Username("guest");
    //             h.Password("guest");
    //         });

    //         //cfg.ReceiveEndpoint("Events:ITickerCollected", e =>
    //         //{
    //         //    e.Consumer<TickerCollectedConsumer>(context);
    //         //});
    //     });
    // });
    // builder.Services.AddMassTransitHostedService();

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

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
