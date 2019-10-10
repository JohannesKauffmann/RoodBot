using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace RoodBot {

	class Program {

		static DiscordClient discord;

		static readonly string token = System.IO.File.ReadAllText("..\\..\\..\\token.txt");

		static void Main(string[] args) {
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		
		static async Task MainAsync(string[] args) {
			discord = new DiscordClient(
				new DiscordConfiguration {
					Token = token,
					TokenType = TokenType.Bot,
					UseInternalLogHandler = true,
					LogLevel = LogLevel.Debug
				}
			);

			discord.ClientErrored += async e => {
				Console.WriteLine("ClientError");
				Console.WriteLine(e.Exception.InnerException.Message);
			};

			// var status = member?.Presence?.Status ?? UserStatus.Offline;

			discord.MessageCreated += async e => {
				DiscordMessage currentMessage = e.Message;
				if (currentMessage.MessageType.Value != MessageType.GuildMemberJoin) {
					//DiscordMember member = await e.Guild.GetMemberAsync(e.Author.Id);
					DiscordMember member = e.Guild.Members.First(x => x.Id == e.Author.Id);
					if (!member.IsCurrent && !member.IsBot) {
						string status;
						if (member.Presence != null) {
							// niet écht offline
							status = member.Presence.Status.ToString();
						} else {
							// je kan niet géén presence hebben en toch een bericht sturen
							// presence == null betekent dat je invis was en daarna discord hebt opgestart
							status = "Offline";
						}
						if (status == "Offline" || status == "Inivisible") {
							string discordTag = member.Username + "#" + member.Discriminator;
							if (member.IsOwner) {
								await currentMessage.RespondAsync("I can't kick you, but you should be for using invisible mode. Fuck you.");
								Console.WriteLine("Unsuccesfully tried to kick " + discordTag);
							} else {
								string reason = "Let " + discordTag + " rejoin so they can redeem themselves";
								DiscordInvite invite = await currentMessage.Channel.CreateInviteAsync(86400, 1, false, true, reason);
								await member.SendMessageAsync(
									"You have been yeeted from " + e.Guild.Name
									+ " for using invisible mode. You get a chance to redeem "
									+ "yourself by using this invite: " + invite.ToString());
								await e.Guild.RemoveMemberAsync(member, reason);
								// await member.RemoveAsync(discordTag + " used invisible mode");
								Console.WriteLine("Succesfully kicked " + discordTag);
							}
						}
					}
				}
			};

			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}
