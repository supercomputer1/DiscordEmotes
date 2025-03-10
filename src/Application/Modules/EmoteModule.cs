using Application.Services;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace Application.Modules;

public class EmoteModule(ILogger<EmoteModule> logger, RequestService requestService)
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

            var emotes = await requestService
                .HandleRequest(requestText, requestType);

            var images = emotes
                .SelectMany(s => s.Images)
                .ToList();

            await FollowupWithFilesAsync(images.Select(i => i.ToFileAttachment()));
        }
        catch (Exception ex)
        {
            logger.LogError("{Ex}", ex);
            await FollowupAsync($"Could not find a emote for query {requestText}.");
        }
    }
}