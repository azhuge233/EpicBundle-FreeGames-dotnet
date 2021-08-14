using System;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace EpicBundle_FreeGames_dotnet {
	class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;

		private const string Url = "https://www.epicbundle.com/category/article/for-free/";

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public HtmlDocument GetHtmlSource(string url = Url) {
			try {
				_logger.LogDebug("Getting page source");
				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(url);
				_logger.LogDebug("Done");
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError("Scraping Error");
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
