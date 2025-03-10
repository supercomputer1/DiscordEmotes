using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Application.Common.Persistence.DataDir);
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure(hostContext.Configuration);
        services.AddApplication(hostContext.Configuration);
    })
    .UseSerilog((hostContext, provider, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Services(provider)
            .WriteTo.Console(theme: SystemConsoleTheme.Literate)
            .WriteTo.File(path: Path.Combine(Application.Common.Persistence.LogDir, "log-.txt"),
                rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext();
    })
    .Build();

await host.RunAsync();