namespace DiscordEmotes.Image;

public class ImageClient(IHttpClientFactory httpClientFactory)
{
    public async Task<Stream?> GetImage(string url)
    {
        var client = httpClientFactory.CreateClient("Images");

        try
        {
            return await client.GetStreamAsync(url);
        }
        catch
        {
            return null;
        }
    }
}