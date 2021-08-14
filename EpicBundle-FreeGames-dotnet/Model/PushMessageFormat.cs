namespace EpicBundle_FreeGames_dotnet.Model {
	public static class PushMessageFormat {
		public static readonly string telegramFormat = "<b>EpicBundle 信息</b>\n\n" +
			"<i>{0}</i>\n" +
			"文章链接: {1}\n" +
			"可能的领取链接:\n";
		public static readonly string barkFormat = "{0}\n文章链接: {1}\n可能的领取链接:\n";
		public static readonly string emailFormat = "<b>{0}</b><br>" +
			"文章链接: {1}<br>" +
			"可能的领取链接:<br>";

		public static readonly string possibleLinkFormat = "{0}\n";
		public static readonly string possibleLinkFormatEmail = "{0}<br>";

		public static readonly string barkUrlFormat = "{0}/{1}/";
		public static readonly string barkUrlTitle = "EpicBundle-FreeGames/";
		public static readonly string barkUrlArgs = "?group=epicbundlefreegames" +
			"&isArchive=1" +
			"&sound=calypso" +
			"&url={0}" +
			"&copy={1}";

		public static readonly string emailTitleFormat = "{0} new free game(s) - EpicBundle-FreeGames";
		public static readonly string emailBodyFormat = "<br>{0}";
	}
}
