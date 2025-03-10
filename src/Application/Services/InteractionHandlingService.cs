using System.Reflection;
using Application.Common;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class InteractionHandlingService : IHostedService
{
    private readonly DiscordSocketClient discord;
    private readonly InteractionService interactions;
    private readonly IServiceProvider services;

    public InteractionHandlingService(
        DiscordSocketClient discord,
        InteractionService interactions,
        IServiceProvider services,
        IConfiguration configuration,
        ILogger<InteractionService> logger)
    {
        this.discord = discord;
        this.interactions = interactions;
        this.services = services;

        this.interactions.Log += msg => LogHelper.OnLogAsync(logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        discord.Ready += () => interactions.RegisterCommandsGloballyAsync(true);
        discord.InteractionCreated += OnInteractionAsync;

        await interactions.AddModulesAsync(Assembly.GetExecutingAssembly(), services);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        interactions.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnInteractionAsync(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(discord, interaction);
            var result = await interactions.ExecuteCommandAsync(context, services);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ToString());
        }
        catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(msg => msg.Result.DeleteAsync());
            }
        }
    }
}