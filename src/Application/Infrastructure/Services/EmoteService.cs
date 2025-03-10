using Application.Infrastructure.Clients;
using Microsoft.Extensions.Logging;

namespace Application.Infrastructure.Services;

public class EmoteService(ILogger<EmoteService> logger, EmoteClient emoteClient)
{
    public async Task<Domain.Models.Emote> GetById(string id)
    {
        var emoteResponse = await emoteClient.GetById(id);

        return Domain.Models.Emote
            .FromEmoteResponse(emoteResponse);
    }

    public async Task<IEnumerable<Domain.Models.Emote>> GetByQuery(string query, int requestLimit = 1)
    {
        // NOTE: For some reason the request sometimes does not find and emote. Even with a 200 response.
        // Thus we try three times.
        for (var i = 0; i < 3; i++)
        {
            var emoteSearchResponse =
                await emoteClient.GetByQuery(query, exactMatch: true, requestLimit: requestLimit) ??
                await emoteClient.GetByQuery(query, exactMatch: false, requestLimit: requestLimit);

            if (emoteSearchResponse is { HasResults: true, Data.Emotes: not null })
            {
                return emoteSearchResponse.Data.Emotes.Items
                    .Select(s => new Domain.Models.Emote(s.Id, s.Name))
                    .ToList();
            }

            logger.LogWarning("Could not find any images for query {Query}.", query);

            await Task.Delay(3000);
        }

        throw new Exception($"Could not find any emotes for query: {query}.");
    }
}