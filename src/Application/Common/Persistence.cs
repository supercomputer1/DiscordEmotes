namespace Application.Common;

public static class Persistence
{
    private const string AppName = "DiscordEmotes";

    public static readonly string DataDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

    private static readonly string EmoteDir =
        Path.Combine(DataDir, "Emotes");

    public static readonly string LogDir =
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
}