using DiscordEmotes.Emote;
using Serilog;

namespace DiscordEmotes.Blabla;

internal static class Persistence
{
    private const string AppName = "DiscordEmotes";

    private static readonly string DataDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

    private static readonly string EmoteDir =
        Path.Combine(DataDir, "Emotes");

    internal static readonly string LogDir =
        Path.Combine(DataDir, "Logs");

    static Persistence()
    {
        if (!Directory.Exists(DataDir))
        {
            Directory.CreateDirectory(DataDir);
        }

        if (!Directory.Exists((EmoteDir)))
        {
            Directory.CreateDirectory(EmoteDir);
        }

        if (!Directory.Exists(LogDir))
        {
            Directory.CreateDirectory(LogDir);
        }
    }

    /// <summary>
    /// Save image to disk.
    /// </summary>
    /// <param name="image"></param>
    public static async Task SaveImage(Image.Models.Image image)
    {
        var path = Path.Combine(EmoteDir, image.FileId);

        var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await image.Stream.CopyToAsync(fileStream);
        await fileStream.DisposeAsync();

        Log.Information("Saved emote {0} to {1}.", image.FileId, path);
    }

    /// <summary>
    /// Get the file path of an emote.
    /// </summary>
    /// <param name="emote"></param>
    /// <returns></returns>
    public static async Task<string> GetEmote(Emote.Models.Emote emote)
    {
        var path = await Task.Run(() => Path.Combine(EmoteDir, emote.Id));
        return path;
    }

    /// <summary>
    /// Get the file stream of an emote.
    /// </summary>
    /// <param name="emote"></param>
    /// <returns></returns>
    public static async Task<Stream> GetEmoteStream(Emote.Models.Emote emote)
    {
        var path = await Task.Run(() => Path.Combine(EmoteDir, emote.Id));
        return File.OpenRead(path);
    }

    /// <summary>
    /// Delete emote from disk.
    /// </summary>
    /// <param name="emote"></param>
    public static async Task RemoveEmote(Emote.Models.Emote emote)
    {
        var path = Path.Combine(EmoteDir, emote.Id);

        await Task.Run(() => File.Delete(path));

        Log.Information("Removed emote {0} from {1}.", emote.Id, path);
    }
}