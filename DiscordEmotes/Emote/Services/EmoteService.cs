using DiscordEmotes.Blabla;
using DiscordEmotes.Emote.Clients;
using DiscordEmotes.Emote.Models;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes.Emote.Services;

public class EmoteService(ILogger<EmoteService> logger, EmoteClient emoteClient)
{
    public async Task<Models.Emote> GetById(string id)
    {
        var emoteResponse = await emoteClient.GetById(id);

        return Models.Emote
            .FromEmoteResponse(emoteResponse);
    }

    public async Task<IEnumerable<Models.Emote>> GetBySetId(string id)
    {
        var emoteSetResponse = await emoteClient.GetBySetId(id);

        return emoteSetResponse.Emotes
            .Select(emote => new Models.Emote(emote.Id, emote.Name))
            .ToList();
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
        
        // get emotes
        return emoteSearchResponse.Data.Emotes.Items
            .Select(s => new Models.Emote(s.Id, s.Name))
            .ToList(); 
    }
}