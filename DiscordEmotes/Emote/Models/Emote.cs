namespace DiscordEmotes.Emote.Models;

public class Emote
{
    private Emote(string id, string name)
    {
        Id = id;
        Name = name; 
    }
    
    public string Id { get; init; }
    public string Name { get; init; }

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