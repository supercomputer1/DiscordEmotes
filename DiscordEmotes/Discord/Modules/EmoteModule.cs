using Discord;
using Discord.Interactions;
using Discord.Interactions.Builders;
using DiscordEmotes.Blabla;
using Microsoft.Extensions.Logging;
using Serilog;
using ModalBuilder = Discord.ModalBuilder;

namespace DiscordEmotes.Discord.Modules;

public class EmoteModule(ILogger<EmoteModule> logger, SomeKindOfService someKindOfService)
    : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("emote", "id")]
    public async Task Id(string id)
    {
        await HandleRequest(id, "id");
    }

    [SlashCommand("search", "query")]
    public async Task Query(string query)
    {
        await HandleRequest(query, "query");
    }

    private async Task HandleRequest(string requestText, string requestType)
    {
        try
        {
            await DeferAsync();
            
            var emotes = await someKindOfService
                .HandleRequest(requestText, requestType);

            var images = emotes
                .SelectMany(s => s.Images)
                .ToList(); 
            
            await FollowupWithFilesAsync(images.Select(i => i.ToFileAttachment()));
            
            // Save file locally 
            foreach (var image in images)
            {
                await Persistence.SaveImage(image);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("{Ex}", ex);
            await FollowupAsync($"Could not find a emote for query {requestText}.");
        }
    }
}