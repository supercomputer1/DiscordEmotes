using System.Collections.ObjectModel;
using DiscordEmotes.Blabla;
using DiscordEmotes.Emote.Clients;
using DiscordEmotes.Emote.Services;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes;

public class SomeKindOfService(
    ILogger<SomeKindOfService> logger,
    EmoteService emoteService,
    ImageService imageService)
{
    // Prioritize gif over png.
    private readonly SortedSet<string> _extensions = [".gif", ".png"];

    public async Task<IEnumerable<Emote.Models.Emote>> HandleRequest(string query, string requestType)
    {
        var emotes = new List<Emote.Models.Emote>();
        switch (requestType)
        {
            case "id":
                emotes.Add(await emoteService.GetById(query));
                break;
            case "query":
                emotes.AddRange(await emoteService.GetByQuery(query));
                break;
        }

        var savedEmotes = new Dictionary<string, Emote.Models.Emote>();

        foreach (var emote in emotes)
        {
            foreach (var extension in _extensions)
            {
                if (savedEmotes.ContainsKey(emote.Id))
                {
                    logger.LogWarning("Emote {Id} already has a saved image. Skipping.", emote.Id);
                    continue;
                }
                
                var image = await imageService.GetImageFromEmote(emote, extension);

                if (image is null)
                {
                    logger.LogWarning("Could not find a emote for extension {Extension}.", extension);
                    continue;
                }

                emote.Extension = extension;
                await Persistence.SaveEmote(emote, image);

                savedEmotes.Add(emote.Id, emote);
            }

            if (!savedEmotes.ContainsKey(emote.Id))
            {
                logger.LogWarning("Could not find any valid images for emote {Id}.", emote.Id);
            }
        }

        return savedEmotes
            .Select(e => e.Value);
    }
}