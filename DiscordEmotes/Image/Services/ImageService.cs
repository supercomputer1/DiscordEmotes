namespace DiscordEmotes.Image.Services;

public class ImageService(ImageClient client)
{
    public async Task<Stream?> GetImageFromEmote(Emote.Models.Emote emote, string extension)
    {
        var stream = await client.GetImage(string.Concat(emote.Url, extension));
        
        return stream; 
    }
}