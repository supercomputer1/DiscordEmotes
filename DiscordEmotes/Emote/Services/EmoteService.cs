using DiscordEmotes.Emote.Clients;
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
        // For some reason the request sometimes does not find an emote. 
        // Thus we try three times.
        for (int i = 0; i < 3; i++)
        {
            var emoteSearchResponse = await emoteClient.GetByQuery(query, exactMatch: true, requestLimit: requestLimit) ??
                                      await emoteClient.GetByQuery(query, exactMatch: false, requestLimit: requestLimit);

            if (emoteSearchResponse is not null && emoteSearchResponse.HasResults && emoteSearchResponse.Data.Emotes is not null)
            {
                return emoteSearchResponse.Data.Emotes.Items
                    .Select(s => new Models.Emote(s.Id, s.Name))
                    .ToList();
            }

            logger.LogWarning("Could not find any images for query {Query}.", query);

            await Task.Delay(500);
        }

        throw new Exception($"Could not find any emotes for query: {query}.");
    }
}