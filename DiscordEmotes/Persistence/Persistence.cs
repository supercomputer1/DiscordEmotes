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
    /// Temporarily save emote to disk.
    /// </summary>
    /// <param name="emote"></param>
    /// <param name="emoteImageStream"></param>
    public static async Task SaveEmote(Emote.Models.Emote emote, Stream emoteImageStream)
    {
        var path = Path.Combine(EmoteDir, emote.FileId);
        
        await using var outputFileStream = new FileStream(path, FileMode.Create);
        await emoteImageStream.CopyToAsync(outputFileStream);    
        
        Log.Information("Saved emote {0} to {1}.", emote.FileId, path);
    }

    /// <summary>
    /// Get the file path of an emote.
    /// </summary>
    /// <param name="emote"></param>
    /// <returns></returns>
    public static async Task<string> GetEmote(Emote.Models.Emote emote)
    {
        var path = await Task.Run(() => Path.Combine(EmoteDir, emote.FileId));
        return path;
    }

    /// <summary>
    /// Get the file stream of an emote.
    /// </summary>
    /// <param name="emote"></param>
    /// <returns></returns>
    public static async Task<Stream> GetEmoteStream(Emote.Models.Emote emote)
    {
        var path = await Task.Run(() => Path.Combine(EmoteDir, emote.FileId));
        return File.OpenRead(path);
    }

    /// <summary>
    /// Delete emote from disk.
    /// </summary>
    /// <param name="emote"></param>
    public static async Task RemoveEmote(Emote.Models.Emote emote)
    {
        var path = Path.Combine(EmoteDir, emote.FileId);

        await Task.Run(() => File.Delete(path));
        
        Log.Information("Removed emote {0} from {1}.", emote.FileId, path);
    }
}