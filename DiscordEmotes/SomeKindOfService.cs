using DiscordEmotes.Emote.Services;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes;

public class SomeKindOfService(
    ILogger<SomeKindOfService> logger,
    EmoteService emoteService,
    ImageService imageService)
{
    public async Task<IEnumerable<Emote.Models.Emote>> HandleRequest(string query, string requestType)
    {
        var emotes = new List<Emote.Models.Emote>();
        switch (requestType)
        {
            case "id":
                emotes.Add(await emoteService.GetById(query));
                break;
            case "query":
                emotes.AddRange(await emoteService.GetByQuery(query, requestLimit: 1));
                break;
        }

        foreach (var emote in emotes)
        {
            var image = await imageService.GetImage(emote.Id);
            emote.AddImage(image);
        }

        return emotes;
    }
}