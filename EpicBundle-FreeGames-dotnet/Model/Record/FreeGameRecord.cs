using System.Text;
using System.Collections.Generic;

namespace EpicBundle_FreeGames_dotnet.Model {

	public class FreeGameRecord {
		public string Url { get; set; }
		public string Title { get; set; }

		public List<string> PossibleLinks { get; set; }

		public string ToTelegramMessage() {
			var sb = new StringBuilder().AppendFormat(NotifyFormatStrings.telegramFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(NotifyFormatStrings.possibleLinkFormat, link));
			return sb.ToString();
		}

		public string ToBarkMessage() {
			var sb = new StringBuilder().AppendFormat(NotifyFormatStrings.barkFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(NotifyFormatStrings.possibleLinkFormat, link));
			return sb.ToString();
		}

		public string ToEmailMessage() {
			var sb = new StringBuilder().AppendFormat(NotifyFormatStrings.emailFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(NotifyFormatStrings.possibleLinkFormatHtml, link));
			return sb.ToString();
		}

		public string ToQQMessage() {
			var sb = new StringBuilder().AppendFormat(NotifyFormatStrings.qqFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(NotifyFormatStrings.possibleLinkFormat, link));
			return sb.ToString();
		}

		public string ToPushPlusMessage() {
			var sb = new StringBuilder().AppendFormat(NotifyFormatStrings.pushPlusFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(NotifyFormatStrings.possibleLinkFormatHtml, link));
			return sb.ToString();
		}

		public string ToDingTalkMessage() {
			var sb = new StringBuilder().AppendFormat(NotifyFormatStrings.dingTalkFormat, Title, Url);
			PossibleLinks.ForEach(link => sb.AppendFormat(NotifyFormatStrings.possibleLinkFormat, link));
			return sb.ToString();
		}
	}
}
