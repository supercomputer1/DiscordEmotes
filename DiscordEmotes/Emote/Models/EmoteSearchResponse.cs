namespace DiscordEmotes.Emote.Models;

public class EmoteSearchResponse
{
    public required EmoteSearchResponseData Data { get; init; }
}

public class EmoteSearchResponseData
{
    public required EmoteSearchResponseEmotes Emotes { get; init; }
}

public class EmoteSearchResponseEmotes
{
    public required List<EmoteSearchResponseItem> Items { get; init; }
}

public class EmoteSearchResponseItem
{
    public required string Id { get; init; }
}