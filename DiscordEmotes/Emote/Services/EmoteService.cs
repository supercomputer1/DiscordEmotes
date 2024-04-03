using DiscordEmotes.Blabla;
using DiscordEmotes.Image;
using DiscordEmotes.Image.Services;
using Microsoft.Extensions.Logging;

namespace DiscordEmotes.Emote.Services;

public class EmoteService(ILogger<EmoteService> logger, EmoteClient emoteClient, ImageService imageService)
{
    public async Task<Models.Emote> GetEmote(string emoteId)
    {
        var emoteResponse = await emoteClient.GetEmoteById(emoteId);
        
        var emotes = Models.Emote.FromEmoteResponse(emoteResponse);

        // Prioritize gif over png.
        foreach (var emote in emotes.OrderBy(o => o.Extension))
        {
            var image = await imageService.GetImageFromEmote(emote);

            if (image is null)
            {
                logger.LogWarning("Could not find a emote for extension {Extension}.", emote.Extension);
                continue;
            }
            
            await Persistence.SaveEmote(emote, image);
            return emote;
        }

        throw new Exception($"No emotes found with valid extensions.");
    }
}