using System.Text;
using System.Collections.Generic;
using EpicBundle_FreeGames_dotnet.Model;

namespace EpicBundle_FreeGames_dotnet.Model {

	public class FreeGameRecord {
		public string Url { get; set; }
		public string Title { get; set; }

		public List<string> PossibleLinks { get; set; }

		public string ToTelegramMessage() {
			var sb = new StringBuilder().AppendFormat(PushMessageFormat.telegramFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(PushMessageFormat.possibleLinkFormat, link));
			return sb.ToString();
		}

		public string ToBarkMessage() {
			var sb = new StringBuilder().AppendFormat(PushMessageFormat.barkFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(PushMessageFormat.possibleLinkFormat, link));
			return sb.ToString();
		}

		public string ToEmailMessage() {
			var sb = new StringBuilder().AppendFormat(PushMessageFormat.emailFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(PushMessageFormat.possibleLinkFormatEmail, link));
			return sb.ToString();
		}
	}
}
