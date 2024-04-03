using Discord.Interactions;
using DiscordEmotes.Blabla;
using DiscordEmotes.Emote;
using DiscordEmotes.Emote.Services;


namespace DiscordEmotes.Discord.Modules;

    public class EmoteModule(EmoteService emoteService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("emote", "enter emoteId.")]
        public async Task Say(string text)
        {
            try
            {
                // Acknowledge the user that the request is received.
                await DeferAsync(); 
                
                var emote = await emoteService.GetEmote(text);
                await FollowupWithFileAsync(await Persistence.GetEmote(emote));
                await Persistence.RemoveEmote(emote);
            }
            catch
            {
                await FollowupAsync($"Could not find a emote for id {text}.");
            }
        }
    }