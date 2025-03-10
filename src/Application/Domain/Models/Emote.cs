using Application.Domain.Contracts;

namespace Application.Domain.Models;

public class Emote(string id, string name)
{
    public string Id { get; init; } = id;
    public string Name { get; init; } = name;

    public readonly List<Image> Images = [];

    public void AddImage(Image image)
    {
        Images.Add(image);
    }

    public static Emote FromEmoteResponse(EmoteContract emoteContract)
    {
        return new Emote(emoteContract.Id, emoteContract.Name);
    }
}