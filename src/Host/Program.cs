using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Application.Common.Persistence.DataDir);
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient("Emotes", client =>
        {
            client.BaseAddress = hostContext.Configuration.GetRequiredValue<Uri>("7TV:BaseAddress");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddStandardResilienceHandler();

        services.AddHttpClient("Images").AddStandardResilienceHandler();

        services.AddTransient<EmoteClient>();
        services.AddTransient<ImageClient>();

        services.AddSingleton<ImageService>();
        services.AddSingleton<EmoteService>();
        services.AddSingleton<RequestService>();
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
        services.AddHostedService<InteractionHandlingService>();
        services.AddHostedService<DiscordStartupService>();
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