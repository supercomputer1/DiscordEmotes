namespace DiscordEmotes.Emote.Models;

public class EmoteSearchResponse
{
    public required EmoteSearchResponseData Data { get; init; }
    public bool HasResults => Data.Emotes is not null; 
}

public class EmoteSearchResponseData
{
    public required EmoteSearchResponseEmotes? Emotes { get; init; }
}

public class EmoteSearchResponseEmotes
{
    public required List<EmoteSearchResponseItem> Items { get; init; }
}

public class EmoteSearchResponseItem
{
    public required string Id { get; init; }
    public required string Name { get; init; }
}