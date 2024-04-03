using Discord;
using Discord.Interactions;
using DiscordEmotes.Blabla;
using DiscordEmotes.Emote;
using DiscordEmotes.Emote.Services;


namespace DiscordEmotes.Discord.Modules;

    public class EmoteModule(SomeKindOfService someKindOfService) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("emote", "id")]
        public async Task Id(string id)
        {
            try
            {
                // Acknowledge the user that the request is received.
                await DeferAsync();

                var emote = await someKindOfService.HandleRequest(id, "id"); 
                    
                await FollowupWithFileAsync(await Persistence.GetEmote(emote.First()));
                await Persistence.RemoveEmote(emote.First());
            }
            catch
            {
                await FollowupAsync($"Could not find a emote for id {id}.");
            }
        }

        [SlashCommand("search", "query")]
        public async Task Query(string query)
        {
            try
            {
                await DeferAsync();

                var emotes = await someKindOfService
                    .HandleRequest(query, "query");
                
                var fileAttachments = await EmoteToFileAttachment(emotes);

                await FollowupWithFilesAsync(fileAttachments);

                foreach (var emote in emotes)
                {
                    await Persistence.RemoveEmote(emote);
                }
            }
            catch
            {
                await FollowupAsync($"Could not find a emote for query {query}.");
            }
            
            
            static async Task<IEnumerable<FileAttachment>> EmoteToFileAttachment(IEnumerable<Emote.Models.Emote> emotes)
            {
                var fileAttachments = new List<FileAttachment>(); 
                foreach (var emote in emotes)
                {
                    fileAttachments.Add(new FileAttachment(stream: await Persistence.GetEmoteStream(emote), fileName: emote.FileId));
                }

                return fileAttachments;
            }
        }
    }