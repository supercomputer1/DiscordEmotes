using DiscordEmotes.Blabla;
using DiscordEmotes.Emote.Clients;
using DiscordEmotes.Emote.Models;
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

    public async Task<IEnumerable<Models.Emote>> GetByQuery(string query, int requestLimit = 1)
    {
        var emoteSearchResponse = await emoteClient.GetByQuery(query, exactMatch: true, requestLimit: requestLimit) ??
                                  await emoteClient.GetByQuery(query, exactMatch: false, requestLimit: requestLimit);

        if (emoteSearchResponse is null || !emoteSearchResponse.HasResults || emoteSearchResponse.Data.Emotes is null)
        {
            logger.LogWarning("Could not find any images for query {Query}.", query);
            return Array.Empty<Models.Emote>(); 
        }
        
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