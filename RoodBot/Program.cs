using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace RoodBot {

	class Program {

		static DiscordClient discord;

		static string token = System.IO.File.ReadAllText("..\\..\\..\\token.txt");

		static void Main(string[] args) {
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		
		static async Task MainAsync(string[] args) {
			discord = new DiscordClient(new DiscordConfiguration {
				Token = token,
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
					DiscordMember member = (DiscordMember) e.Author;
					if (status == "Offline" || status == "Inivisible") {
						try {
							string message = "Darn it, " + member.DisplayName.ToString() + ", you have to be online in order to send messages!";
							// await member.RemoveAsync(message);
							DiscordDmChannel channel = await member.CreateDmChannelAsync();
							// DiscordGuild guild = e.Guild;
							await channel.SendMessageAsync(message);
						} catch (DSharpPlus.Exceptions.UnauthorizedException f) {
							Console.WriteLine(f.JsonMessage);
						}
					}
				}
			};

			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}
