namespace Application.Infrastructure.Clients;

public class ImageClient(IHttpClientFactory httpClientFactory)
{
    private const string ImageClientName = "Images";
    public async Task<Stream?> GetImage(string url)
    {
        var client = httpClientFactory.CreateClient(ImageClientName);

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