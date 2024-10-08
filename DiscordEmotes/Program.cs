﻿using System.Net;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordEmotes;
using DiscordEmotes.Common;
using DiscordEmotes.Common.Extensions;
using DiscordEmotes.Discord.Services;
using DiscordEmotes.Emote.Clients;
using DiscordEmotes.Emote.Services;
using DiscordEmotes.Image.Clients;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        //// 408, 5xx
        .HandleTransientHttpError()
        //// 404
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        //// 401
        .OrResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));
}

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Persistence.DataDir);
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient("Emotes", client =>
        {
            client.BaseAddress = hostContext.Configuration.GetRequiredValue<Uri>("7TV:BaseAddress");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient("Images");

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
            .WriteTo.File(path: Path.Combine(Persistence.LogDir, "log-.txt"), rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext();
    })
    .Build();

await host.RunAsync();

