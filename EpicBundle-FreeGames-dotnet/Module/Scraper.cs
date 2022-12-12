using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace EpicBundle_FreeGames_dotnet {
	class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;

		private const string Url = "https://www.epicbundle.com/category/article/for-free/";

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
			Microsoft.Playwright.Program.Main(new string[] { "install", "firefox" });
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

		public async Task<HtmlDocument> GetHtmlSourceWithPlaywright(string url = Url) {
			try {
				_logger.LogDebug("Getting page source with Playwright");

				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Firefox.LaunchAsync(new() { Headless = true });

				var page = await browser.NewPageAsync();

				await page.GotoAsync(url);
				await page.WaitForLoadStateAsync();

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(await page.InnerHTMLAsync("*"));

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
