namespace DiscordEmotes.Emote.Models;

public class Emote
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Url { get; init; }
    public string Extension { get; set; }
    public string FileId => string.Concat(Id, Extension);

    public static IEnumerable<Emote> FromEmoteResponse(EmoteResponse emoteResponse)
    {
        HashSet<string> goodFileExtensions = [".webp"];
        var image = emoteResponse.Host.Files.Where(f => goodFileExtensions.Contains(f.FileExtension)).MaxBy(o => o.Height);

        if (image is null)
        {
            throw new Exception($"Could not find a valid image file.");
        }
        
        var url = string.Concat("https:", emoteResponse.Host.Url, "/", image.NameWithoutExtension);

        var emotePng = new Emote()
        {
            Id = emoteResponse.Id,
            Name = emoteResponse.Name,
            Url = url,
            Extension = ".png"
        };

        var emoteGif = new Emote()
        {
            Id = emoteResponse.Id,
            Name = emoteResponse.Name,
            Url = url,
            Extension = ".gif"
        };


        return new List<Emote> { emoteGif, emotePng };
    }
}
