using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EpicBundle_FreeGames_dotnet.Module;
using EpicBundle_FreeGames_dotnet.Model;

namespace EpicBundle_FreeGames_dotnet {
	class Parser : IDisposable {
		#region DI
		private readonly ILogger<Parser> _logger;
		private readonly IServiceProvider services = DI.BuildDiScraperOnly();
		#endregion

		#region debug strings
		private readonly string debugTryGetLinks = "Getting article page source";
		private readonly string debugRetryWithUlXPath = "Didn't get any links using linkspXPath, retrying with linksulXPath";
		private readonly string debugGetPossibleLink = "Get possible link: {0}";
		private readonly string debugGetGetUrlFromLinks = "Get possible link: {0}";
		#endregion

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		private List<string> GetUrlFromLinks(HtmlNodeCollection links) {
			_logger.LogDebug(debugGetGetUrlFromLinks);
			var results = new List<string>();

			foreach (var each in links) {
				string possibleLink = each.Attributes["href"].Value.Split('?')[0];
				string data_wpel_link = each.Attributes["data-wpel-link"].Value;
				if (data_wpel_link == "external" && !ParseString.wordList.Exists(x => x == each.InnerText.ToString().ToLower()) && !ParseString.urlList.Exists(x => (x == possibleLink.ToLower() || possibleLink.ToLower().Contains(x)))) {
					_logger.LogDebug(debugGetPossibleLink, possibleLink);
					results.Add(possibleLink);
				}
			}

			_logger.LogDebug($"Done: {debugGetGetUrlFromLinks}");
			return results;
		}

		public List<string> TryGetLinks(string url) { 
			_logger.LogDebug(debugTryGetLinks);

			try {
				var htmlDoc = services.GetRequiredService<Scraper>().GetHtmlSource(url);

				var result = GetUrlFromLinks(htmlDoc.DocumentNode.SelectNodes(ParseString.linkspXPath));

				if (result.Count == 0) {
					_logger.LogDebug(debugRetryWithUlXPath);

					result = GetUrlFromLinks(htmlDoc.DocumentNode.SelectNodes(ParseString.linksulXPath));
				}

				_logger.LogDebug($"Done: {debugTryGetLinks}");
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {debugGetGetUrlFromLinks}");
				throw;
			}
		}

		public ParseResult Parse(HtmlDocument htmlDoc, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug("Start parsing");
				var parseResult = new ParseResult();

				var articles = htmlDoc.DocumentNode.SelectNodes(ParseString.articlesXPath);

				foreach (var each in articles) {
					// get article titles and links
					var title = each.InnerText;
					var link = each.Attributes["href"].Value;

					_logger.LogInformation("Found new info: {0}", title);

					// save titles and links to List
					var newRecord = new FreeGameRecord {
						Title = title,
						Url = link,
						PossibleLinks = TryGetLinks(link)
					};

					parseResult.RecordList.Add(newRecord);

					// push list
					if (records.Count == 0 || !records.Exists(x => x.Title == title && x.Url == link)) {
						_logger.LogInformation("Add {0} to push list\n", link);
						parseResult.PushList.Add(newRecord);
					} else _logger.LogInformation("{0} is found in previous records, stop adding it to push list\n", link);
				}

				_logger.LogDebug("Done");
				return parseResult;
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
