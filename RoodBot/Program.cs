using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace RoodBot {
	class Program {
		static DiscordClient discord;
		
		static void Main(string[] args) {
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		
		static async Task MainAsync(string[] args) {
			discord = new DiscordClient(new DiscordConfiguration {
				Token = "NTM0MDYwOTU2MjM4NzQxNTE1.Dx0TTQ.UkR9hbwTElGiib8DDnLNYqEPUHs",
				TokenType = TokenType.Bot,
				UseInternalLogHandler = true,
				LogLevel = LogLevel.Debug
			});
			
			discord.MessageCreated += async e => {
				if (!e.Message.Author.IsCurrent && !e.Message.Author.IsBot) {
					string status;
					if (e.Message.Author.Presence != null) {
						// niet écht offline
						status = e.Message.Author.Presence.Status.ToString();
					} else {
						// je kan niet géén presence hebben en toch een bericht sturen
						// presence == null betekent dat je invis was en daarna discord hebt opgestart
						status = "Offline";
					}
					if (status == "Offline" || status == "Inivisible") {
						//TODO: kick
						await e.Message.RespondAsync(status);
					}
				}
			};
			
			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}
