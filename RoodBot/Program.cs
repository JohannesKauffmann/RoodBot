using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace RoodBot
{
    class Program
    {
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NTM0MDYwOTU2MjM4NzQxNTE1.Dx0TTQ.UkR9hbwTElGiib8DDnLNYqEPUHs",
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("jij wat maat"))
                    await e.Message.RespondAsync("G E K O L O N I S E E R D");
            };

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
