using Discord;
using Discord.WebSocket;
using DiscordEmotes.Common.Extensions;
using DiscordEmotes.Discord.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes.Discord.Services;

public class DiscordStartupService : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly IConfiguration _configuration;

    public DiscordStartupService(DiscordSocketClient discord, IConfiguration configuration,
        ILogger<DiscordSocketClient> logger)
    {
        _discord = discord;
        _configuration = configuration;

        _discord.Log += msg => LogHelper.OnLogAsync(logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _discord.LoginAsync(TokenType.Bot, _configuration.GetRequiredValue<string>("Discord:Token"));
        await _discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discord.LogoutAsync();
        await _discord.StopAsync();
    }
}