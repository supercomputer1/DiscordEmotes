using Discord.Interactions;
using Discord.WebSocket;
using DiscordEmotes.Blabla;
using DiscordEmotes.Discord.Services;
using DiscordEmotes.Emote;
using DiscordEmotes.Emote.Services;
using DiscordEmotes.Image;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

// TODO: 
// Make it possible to search for emotes.
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient("Emotes", client =>
        {
            client.BaseAddress = new Uri(hostContext.Configuration["7TV:BaseAddress"] ?? throw new NullReferenceException("Missing EmoteClient BaseAddress in configuration."));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("Images");
        
        services.AddSingleton<EmoteClient>();
        services.AddSingleton<ImageClient>();
        services.AddSingleton<EmoteService>(); 
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();
        services.AddHostedService<InteractionHandlingService>(); 
        services.AddHostedService<DiscordStartupService>(); 
    })
    .UseSerilog((hostContext, provider, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Services(provider)
            .WriteTo.Console(theme: SystemConsoleTheme.Literate)
            .WriteTo.File(path: Path.Combine(Persistence.LogDir, "log-.txt"), rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext();
    })
    .Build();

await host.RunAsync();