// ReSharper disable ClassNeverInstantiated.Global

namespace DiscordEmotes.Emote.Models;

public class EmoteResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required EmoteHost Host { get; init; }
}
 
public class EmoteHost
{
    public required string Url { get; init; }
    public required IEnumerable<EmoteFile> Files { get; init; }
}

public class EmoteFile
{
    public required string Name { get; init; }
    public string NameWithoutExtension => Name.Remove(Name.IndexOf('.'));
    public string FileExtension => Name[Name.IndexOf('.')..];
    public required int Width { get; init; }
    public required int Height { get; init; }
}
