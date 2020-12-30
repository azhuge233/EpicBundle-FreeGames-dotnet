using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace EpicBundle_FreeGames_dotnet {
	public class TgBot: IDisposable {
		private TelegramBotClient BotClient { get; set; }

		public TgBot(string token) {
			this.BotClient = new TelegramBotClient(token: token);
		}

		public async Task SendMessage(string chatId, string msg, bool htmlMode = false) {
			if (msg != string.Empty) {
				await BotClient.SendTextMessageAsync(
					chatId: chatId,
					text: msg,
					parseMode: htmlMode ? ParseMode.Html : ParseMode.Default
				);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
