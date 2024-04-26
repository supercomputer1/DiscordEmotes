using DiscordEmotes.Emote.Services;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes;

public class RequestService(ILogger<RequestService> logger, EmoteService emoteService, ImageService imageService)
{
    public async Task<IEnumerable<Emote.Models.Emote>> HandleRequest(string query, string requestType)
    {
        logger.LogInformation("Handling request of type {RequestType} with data {Query}.", requestType, query);

        var emotes = new List<Emote.Models.Emote>();
        switch (requestType)
        {
            case "id":
                emotes.Add(await emoteService.GetById(query));
                break;
            case "query":
                emotes.AddRange(await emoteService.GetByQuery(query, requestLimit: 1));
                break;
            case "set":
                emotes.AddRange(await emoteService.GetBySetId(query));
                break;
        }

        // Discord has a limit of max 10 files per message.
        foreach (var emote in emotes.Take(10))
        {
            var image = await imageService.GetImage(emote.Id);
            emote.AddImage(image);
        }

        return emotes;
    }
}