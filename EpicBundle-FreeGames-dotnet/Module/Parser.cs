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

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public List<string> TryGetLinks(string url) {
			var results = new List<string>();

			// getting all possible links
			_logger.LogDebug("Getting article page source");
			var htmlDoc = services.GetRequiredService<Scraper>().GetHtmlSource(url);

			var links = htmlDoc.DocumentNode.SelectNodes(ParseString.linksXPath);

			// add links to list
			foreach (var each in links) {
				string possibleLink = each.Attributes["href"].Value.Split('?')[0];
				string data_wpel_link = each.Attributes["data-wpel-link"].Value;
				if (data_wpel_link == "external" && !ParseString.wordList.Exists(x => x == each.InnerText.ToString().ToLower()) && !ParseString.urlList.Exists(x => x == possibleLink.ToLower())) {
					_logger.LogDebug("Get possible link: {0}", possibleLink);
					results.Add(possibleLink);
				}
			}

			_logger.LogDebug("Done");
			return results;
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
