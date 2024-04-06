using DiscordEmotes.Image.Clients;

namespace DiscordEmotes.Image.Services;

public class ImageService(ImageClient client)
{
    private const string BaseUri = "https://cdn.7tv.app/emote/";
    private const string Size = "4x";
    private readonly SortedSet<string> _extensions = [".gif", ".png"];

    public async Task<Models.Image> GetImage(string id)
    {
        foreach (var extension in _extensions)
        {
            var url = string.Concat(BaseUri, id, "/", Size, extension);
            var stream = await client.GetImage(url);

            if (stream != null)
            {
                return new Models.Image(id, url, stream, Size, extension);
            }
        }

        throw new Exception($"Could not find any images with a suitable extension for emote {id}.");
    }
}