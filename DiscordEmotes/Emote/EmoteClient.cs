using System.Text.Json;
using DiscordEmotes.Emote.Extensions;
using DiscordEmotes.Emote.Models;

namespace DiscordEmotes.Emote;

public class EmoteClient(IHttpClientFactory httpClientFactory)
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<EmoteResponse> GetEmoteById(string emoteId)
    {
        var client = httpClientFactory.CreateClient("Emotes");
        var emote = await client.GetAsync($"emotes/{emoteId}");

        if (!emote.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Emote {emoteId} was not found.");
        }

        var stream = await emote.Content.ReadAsStreamAsync();
        return stream.DeserializeNotNull<EmoteResponse>(_jsonSerializerOptions);
    }


}