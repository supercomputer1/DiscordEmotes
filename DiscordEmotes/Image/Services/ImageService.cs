namespace DiscordEmotes.Image.Services;

public class ImageService(ImageClient client)
{
    public async Task<Stream?> GetImageFromEmote(Emote.Models.Emote emote)
    {
        var stream = await client.GetImage(string.Concat(emote.Url, emote.Extension));
        
        return stream; 
    }
}