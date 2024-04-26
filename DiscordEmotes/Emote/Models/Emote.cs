namespace DiscordEmotes.Emote.Models;

public class Emote(string id, string name)
{
    public string Id { get; init; } = id;
    public string Name { get; init; } = name;

    public List<Image.Models.Image> Images = [];

    public void AddImage(Image.Models.Image image)
    {
        Images.Add(image);
    }

    public static Emote FromEmoteResponse(EmoteResponse emoteResponse)
    {
        return new Emote(emoteResponse.Id, emoteResponse.Name);
    }
}