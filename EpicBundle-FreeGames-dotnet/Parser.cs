using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace EpicBundle_FreeGames_dotnet {
	class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;
		private readonly IServiceProvider serviceProvider = Program.BuildDi();
		private readonly string pushFormat = "<b>EpicBundle 信息</b>\n\n<i>{0}</i>\n文章链接: {1}\n可能的领取链接:\n";
		private readonly List<string> wordList = new()  {
			"humble choice bundle",
			"follow us on facebook",
			"more games for free here",
			"\"limitless $10 epic coupons\"",
			"add your comment."
		};

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public List<string> TryGetLinks(string url) {
			var results = new List<string>();

			// getting all possible links
			_logger.LogDebug("Getting article page source");
			var scraper = serviceProvider.GetRequiredService<Scraper>();
			var htmlDoc = scraper.GetHtmlSource(url);

			var links = htmlDoc.DocumentNode.CssSelect("div.entry-content a");

			// add links to list
			foreach (var each in links) {
				if (!wordList.Exists(x => x == each.InnerText.ToString().ToLower())) {
					string link = each.Attributes["href"].Value.ToString().Split('?')[0];

					if (link.Contains("#disqus_thread")) continue;

					_logger.LogInformation("Get possible link: {0}", link);
					results.Add(link);
				}
			}

			_logger.LogDebug("Done");
			return results;
		}

		public Tuple<List<string>, List<Dictionary<string, string>>> Parse(HtmlDocument htmlDoc, List<Dictionary<string, string>> records) {
			try {
				_logger.LogDebug("Start parsing");
				var pushList = new List<string>();
				var recordList = new List<Dictionary<string, string>>();

				var articles = htmlDoc.DocumentNode.CssSelect("div.post-column article header h2 a");

				foreach (var each in articles) {
					// get article titles and links
					var title = each.InnerText.ToString();
					var link = each.Attributes["href"].Value.ToString();

					_logger.LogInformation("Found new info: {0}", title);

					// save titles and links to List
					var tmp = new Dictionary<string, string> {
						{ "title", title },
						{ "url", link}
					};
					recordList.Add(tmp);

					// push list
					if (!records.Where(x => x["title"] == title && x["url"] == link).Any()) {
						_logger.LogInformation("Add {0} to push list", title);

						//try to get links
						var possibleLinks = TryGetLinks(url: link);

						StringBuilder sb = new();
						sb.AppendFormat(pushFormat, title, link);

						foreach (var i in possibleLinks)
							sb.AppendFormat("{0}\n", i);

						pushList.Add(sb.ToString());
					} else _logger.LogInformation("{0} is found in previous records, stop adding it to push list", title);
				}

				_logger.LogDebug("Done");
				return new Tuple<List<string>, List<Dictionary<string, string>>>(pushList, recordList);
			} catch (Exception) {
				_logger.LogError("Parsing Error");
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
