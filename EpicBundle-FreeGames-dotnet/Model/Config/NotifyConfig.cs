﻿namespace EpicBundle_FreeGames_dotnet.Model {
	public class NotifyConfig {
		public bool EnableTelegram { get; set; }
		public bool EnableBark { get; set; }
		public bool EnableEmail { get; set; }

		public string TelegramToken { get; set; }
		public string TelegramChatID { get; set; }

		public string BarkAddress { get; set; }
		public string BarkToken { get; set; }

		public string SMTPServer { get; set; }
		public int SMTPPort { get; set; }
		public string FromEmailAddress { get; set; }
		public string ToEmailAddress { get; set; }
		public string AuthAccount { get; set; }
		public string AuthPassword { get; set; }
	}
}