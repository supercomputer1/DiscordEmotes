namespace DiscordEmotes.Emote.Models;

public class EmoteSetResponse
{
    public required IEnumerable<EmoteResponse> Emotes { get; init; }
}