using System.Text;
using System.Text.Json;
using DiscordEmotes.Emote.Extensions;
using DiscordEmotes.Emote.Models;

namespace DiscordEmotes.Emote.Clients;

public class EmoteClient(IHttpClientFactory httpClientFactory)
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private const string EmoteClientName = "Emotes";

    public async Task<EmoteResponse> GetById(string emoteId)
    {
        var client = httpClientFactory.CreateClient(EmoteClientName);
        var emote = await client.GetAsync($"emotes/{emoteId}");
        if (!emote.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Emote {emoteId} was not found.");
        }

        var stream = await emote.Content.ReadAsStreamAsync();
        return stream.DeserializeNotNull<EmoteResponse>(_jsonSerializerOptions);
    }

    public async Task<EmoteSetResponse> GetBySetId(string id)
    {
        var client = httpClientFactory.CreateClient(EmoteClientName);
        var emote = await client.GetAsync($"emote-sets/{id}");

        if (!emote.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Emote-Set {id} was not found.");
        }

        var stream = await emote.Content.ReadAsStreamAsync();
        return stream.DeserializeNotNull<EmoteSetResponse>(_jsonSerializerOptions);
    }

    public async Task<EmoteSearchResponse?> GetByQuery(string query, bool exactMatch, int requestLimit)
    {
        var client = httpClientFactory.CreateClient(EmoteClientName);

        var payload = CreatePayload(query, exactMatch, requestLimit);
        var response = await client.PostAsync(new Uri("gql", UriKind.Relative), payload);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStreamAsync();
        var emoteSearchResponse = responseContent.DeserializeNotNull<EmoteSearchResponse>(_jsonSerializerOptions);

        return emoteSearchResponse.HasResults ? emoteSearchResponse : null;
    }

    private static StringContent CreatePayload(string query, bool exactMatch, int requestLimit)
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
                query,
                limit = requestLimit,
                page = 1,
                sort = new { value = "popularity", order = "DESCENDING" },
                filter = new
                {
                    category = "TOP",
                    exact_match = exactMatch,
                    case_sensitive = false,
                    ignore_tags = false,
                    zero_width = false,
                    animated = true,
                    aspect_ratio = ""
                }
            },

            query = queries["all"]
        };

        return new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
    }
}