using DiscordEmotes.Blabla;
using DiscordEmotes.Image;

namespace DiscordEmotes.Emote.Services;

public class EmoteService(EmoteClient emoteClient, ImageClient imageClient)
{
    public async Task<string> GetEmote(string emoteId)
    {
        var emote = await emoteClient.GetEmoteById(emoteId);
        var highestQualityEmote = emote.GetImage();

        var image = await TryGetImage(highestQualityEmote, ".gif") ?? await TryGetImage(highestQualityEmote, ".png");
        await Persistence.SaveEmote(highestQualityEmote, image);
        return await Persistence.GetEmote(highestQualityEmote);

        async Task<Stream?> TryGetImage(EmoteTest emote, string extension)
        {
            emote.Extension = extension;

            return await imageClient.GetImage(string.Concat(emote.Url, emote.Extension));
        }
    }
}