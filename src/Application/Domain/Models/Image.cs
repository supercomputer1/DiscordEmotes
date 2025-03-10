using Discord;

namespace Application.Domain.Models;

public class Image(string id, string url, Stream stream, string size, string extension)
{
    public string Id { get; init; } = id;
    public string Url { get; init; } = url;
    public Stream Stream { get; init; } = stream;
    public string Size { get; init; } = size;
    public string Extension { get; init; } = extension;
    public string FileId => string.Concat(Id, Extension);

    public FileAttachment ToFileAttachment()
    {
        return new FileAttachment(stream: Stream, fileName: FileId);
    }
}