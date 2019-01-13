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
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync(e.Message.Timestamp.ToString());
            };

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
