using Discord;

namespace DiscordEmotes.Image.Models;

public class Image
{
    public Image(string id, string url, Stream stream, string size, string extension)
    {
        Id = id;
        Url = url;
        Stream = stream;
        Size = size;
        Extension = extension; 
    }
    
    public string Id { get; init; }
    public string Url { get; init; }
    public Stream Stream { get; init; }
    public string Size { get; init; }
    public string Extension { get; init; }
    public string FileId => string.Concat(Id, Extension);

    public FileAttachment ToFileAttachment()
    {
        return new FileAttachment(stream: Stream, fileName: FileId);
    }
}