using Discord.Interactions;
using DiscordEmotes.Blabla;
using DiscordEmotes.Emote;
using DiscordEmotes.Emote.Services;


namespace DiscordEmotes.Discord.Modules;

    public class EmoteModule(EmoteService emoteService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("emote", "say something.")]
        public async Task Say(string text)
        {
            var emote = await emoteService.GetEmote(text); 
            await RespondWithFileAsync(emote);
        }
    }