using Application.Common;
using Application.Common.Extensions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class DiscordStartupService : IHostedService
{
    private readonly DiscordSocketClient discord;
    private readonly IConfiguration configuration;

    public DiscordStartupService(DiscordSocketClient discord, IConfiguration configuration,
        ILogger<DiscordSocketClient> logger)
    {
        this.discord = discord;
        this.configuration = configuration;

        this.discord.Log += msg => LogHelper.OnLogAsync(logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await discord.LoginAsync(TokenType.Bot, configuration.GetRequiredValue<string>("Discord:Token"));
        await discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discord.LogoutAsync();
        await discord.StopAsync();
    }
}