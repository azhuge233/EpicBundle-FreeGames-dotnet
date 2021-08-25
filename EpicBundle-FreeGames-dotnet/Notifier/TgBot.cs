using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using EpicBundle_FreeGames_dotnet.Notifier;
using EpicBundle_FreeGames_dotnet.Model;

namespace EpicBundle_FreeGames_dotnet {
	public class TgBot : INotifiable {
		private readonly ILogger<TgBot> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Telegram";
		#endregion

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			var BotClient = new TelegramBotClient(token: config.TelegramToken);

			try {
				foreach (var record in records) {
					_logger.LogDebug($"{debugSendMessage} : {record.Title}");
					await BotClient.SendTextMessageAsync(
						chatId: config.TelegramChatID,
						text: $"{record.ToTelegramMessage()}{NotifyFormatStrings.projectLinkHTML.Replace("<br>", "\n")}",
						parseMode: ParseMode.Html
					);
				}
			} catch (Exception) {
				_logger.LogError("Send notification failed.");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
