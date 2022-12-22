using System.Collections.Generic;

namespace EpicBundle_FreeGames_dotnet.Model {
	public static class ParseString {
		#region black lists
		public static readonly List<string> wordList = new() {
			"humble choice bundle",
			"follow us on facebook",
			"more games for free here",
			"\"limitless $10 epic coupons\"",
			"add your comment."
		};
		public static readonly List<string> urlList = new() {
			"https://www.humblebundle.com/subscription",
			"https://www.youtube.com/watch",
			"metacritic.com",
			"https://www.facebook.com/epicbundle",
			"https://twitter.com/epicbundlecom",
			"https://www.humblebundle.com/",
			"/aff_c"
		};
		public static readonly List<string> adDomains = new() {
			"adtraction.com",
			"dpbolvw.net"
		};
		#endregion

		#region XPath strings
		public static readonly string articlesXPath = ".//div[contains(@class,\"entry-wrap\")]/header/h2/a";
		public static readonly string linkspXPath = ".//div[contains(@class,\"entry-content\")]/p/a";
		public static readonly string linksh2XPath = ".//div[contains(@class,\"entry-content\")]/h2/a";
		public static readonly string linksulXPath = ".//div[contains(@class,\"entry-content\")]/ul/li/a";
		#endregion
	}
}
