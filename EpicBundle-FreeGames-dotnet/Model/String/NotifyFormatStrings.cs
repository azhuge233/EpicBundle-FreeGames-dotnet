namespace EpicBundle_FreeGames_dotnet.Model {
	public static class NotifyFormatStrings {
		#region ToMessage() strings
		public static readonly string telegramFormat = "<b>EpicBundle 信息</b>\n\n" +
			"<i>{0}</i>\n" +
			"文章链接: {1}\n" +
			"可能的领取链接:\n";
		public static readonly string barkFormat = "{0}\n" +
			"文章链接: {1}\n" +
			"可能的领取链接:\n";
		public static readonly string emailFormat = "<b>{0}</b><br>" +
			"文章链接: <a href=\"{1}\">{1}</a><br>" +
			"可能的领取链接:<br>";
		public static readonly string qqFormat = "EpicBundle 信息\n\n" +
			"{0}\n" +
			"文章链接: {1}\n" +
			"可能的领取链接:\n";
		public static readonly string pushPlusFormat = "<b>{0}</b><br>" +
			"文章链接: <a href=\"{1}\">{1}</a><br>" +
			"可能的领取链接:<br>";
		public static readonly string dingTalkFormat = "EpicBundle 信息\n\n" +
			"{0}\n" +
			"文章链接: {1}\n" +
			"可能的领取链接:\n";
		public static readonly string pushDeerFormat = "EpicBundle 信息\n\n" +
			"{0}\n" +
			"文章链接: {1}\n" +
			"可能的领取链接:\n";
		public static readonly string discordFprmat = "文章链接: {0}\n" +
			"可能的领取链接:\n";
		#endregion

		#region url, title format strings
		public static readonly string possibleLinkFormat = "{0}\n";
		public static readonly string possibleLinkFormatHtml = "<a href=\"{0}\">{0}</a><br>";

		public static readonly string telegramTag = "\n#EpicBundle";

		public static readonly string barkUrlFormat = "{0}/{1}/";
		public static readonly string barkUrlTitle = "EpicBundle-FreeGames/";
		public static readonly string barkUrlArgs = "?group=epicbundlefreegames" +
			"&isArchive=1" +
			"&sound=calypso" +
			"&url={0}" +
			"&copy={1}";

		public static readonly string emailTitleFormat = "{0} new free game(s) - EpicBundle-FreeGames";
		public static readonly string emailBodyFormat = "<br>{0}";

		public static readonly string qqUrlFormat = "http://{0}:{1}/send_private_msg?user_id={2}&message=";

		public static readonly string pushPlusTitleFormat = "{0} new free game(s) - EpicBundle-FreeGames";
		public static readonly string pushPlusBodyFormat = "<br>{0}";
		public static readonly string pushPlusUrlFormat = "http://www.pushplus.plus/send?token={0}&template=html&title={1}&content=";

		public static readonly string dingTalkUrlFormat = "https://oapi.dingtalk.com/robot/send?access_token={0}";

		public static readonly string pushDeerUrlFormat = "https://api2.pushdeer.com/message/push?pushkey={0}&&text={1}";
		#endregion

		public static readonly string projectLink = "\n\nFrom https://github.com/azhuge233/EpicBundle-FreeGames-dotnet";
		public static readonly string projectLinkHTML = "<br><br>From <a href=\"https://github.com/azhuge233/EpicBundle-FreeGames-dotnet\">EpicBundle-FreeGames</a>";
	}
}
