using GrpcGreater.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var webApplicationOptions = new WebApplicationOptions() 
    { 
        ContentRootPath = AppContext.BaseDirectory, 
        Args = args, 
        ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName 
    };
    var builder = WebApplication.CreateBuilder(webApplicationOptions);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Host.UseWindowsService();

    // Additional configuration is required to successfully run gRPC on macOS.
    // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

    // Add services to the container.
    builder.Services.AddGrpc();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    app.MapGrpcService<GreeterService>(); //.RequireHost("*:5001")
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

