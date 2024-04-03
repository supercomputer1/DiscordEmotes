// ReSharper disable ClassNeverInstantiated.Global

namespace DiscordEmotes.Emote;

public class EmoteResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required EmoteHost Host { get; init; }

    public EmoteTest GetImage()
    {
        HashSet<string> goodFileExtensions = [".webp"];
        var image = Host.Files.Where(f => goodFileExtensions.Contains(f.FileExtension)).MaxBy(o => o.Height);

        if (image is null)
        {
            throw new Exception();
        }
        
        var url = string.Concat("https:", Host.Url, "/", image.NameWithoutExtension);
        return new EmoteTest()
        {
            Id = Id,
            Name = Name,
            Url = url
        };
    }
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

public class EmoteTest
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Url { get; init; }
    public string Extension { get; set; }
    public string FileId => string.Concat(Id, Extension);
}