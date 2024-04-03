using System.Text;
using System.Text.Json;
using DiscordEmotes.Emote.Extensions;
using DiscordEmotes.Emote.Models;

namespace DiscordEmotes.Emote.Clients;

public class EmoteClient(IHttpClientFactory httpClientFactory)
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<EmoteResponse> GetById(string emoteId)
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

    public async Task<EmoteSearchResponse> GetByQuery(string query)
    {
        var queries = new Dictionary<string, string>
        {
            ["all"] = "query SearchEmotes($query: String!, $page: Int, $sort: Sort, $limit: Int, $filter: EmoteSearchFilter) {\n emotes(query: $query, page: $page, sort: $sort, limit: $limit, filter: $filter) {\nitems{\n id\n name\n owner{\n username\n }\n host{\n url}}\n}\n}"
        };
        
        var payload = new
        {
            operationName = "SearchEmotes",
            variables = new
            {
                query = query,
                limit = 1,
                page = 1,
                sort = new { value = "popularity", order = "DESCENDING" },
                filter = new
                {
                    category = "TOP",
                    exact_match = false,
                    case_sensitive = false,
                    ignore_tags = false,
                    zero_width = false,
                    animated = true,
                    aspect_ratio = ""
                }
            },
                
            query = queries["all"]
        };
        
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var client = httpClientFactory.CreateClient("Emotes");
        
        var response = await client.PostAsync(new Uri("gql", UriKind.Relative), content);
        var responseContent = await response.Content.ReadAsStreamAsync();
        return responseContent.DeserializeNotNull<EmoteSearchResponse>(_jsonSerializerOptions);
    }
}