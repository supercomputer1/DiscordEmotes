using DiscordEmotes.Blabla;
using DiscordEmotes.Emote.Clients;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes.Emote.Services;

public class EmoteService(ILogger<EmoteService> logger, EmoteClient emoteClient, ImageService imageService)
{
    public async Task<Models.Emote> GetById(string id)
    {
        var emoteResponse = await emoteClient.GetById(id);

        return Models.Emote
            .FromEmoteResponse(emoteResponse);
    }

    public async Task<IEnumerable<Models.Emote>> GetByQuery(string query)
    {
        var emoteSearchResponse = await emoteClient.GetByQuery(query);

        // get ids
        var emoteIds = emoteSearchResponse.Data.Emotes.Items.Select(s => s.Id);

        var emotes = new List<Models.Emote>();
        foreach (var id in emoteIds)
        {
            var emoteResponse = await emoteClient.GetById(id);
            emotes.Add(Models.Emote.FromEmoteResponse(emoteResponse));
        }

        return emotes;
    }
}