using System;
using System.Net.Http;
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

		public async Task<HtmlDocument> GetHtmlSource(string url = Url) {
			try {
				_logger.LogDebug("Getting Html Source");
				string content = string.Empty;
				bool retryed = false;

				try {
					content = await GetHtmlSourceWithRegularGetRequest(url);
				} catch (Exception ex) {
					_logger.LogError("Error: Getting source with regular GET request, retrying with playwright");
					_logger.LogError(ex.Message);

					try {
						content = await GetHtmlSourceWithPlaywright(url);
					} catch (Exception) {
						content = string.Empty;
					} finally {
						retryed = true;
					}	
				}

				if (!retryed && string.IsNullOrEmpty(content)) {
					try {
						content = await GetHtmlSourceWithPlaywright(url);
					} catch (Exception) {
						content = string.Empty;
					}
				}

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(content);

				_logger.LogDebug("Done");
				return htmlDoc;
			} catch(Exception) {
				_logger.LogError("Scraping Error: Get Source");
				throw;
			}
		}

		public async Task<string> GetHtmlSourceWithRegularGetRequest(string url = Url) {
			try {
				_logger.LogDebug("Getting page source with regular GET request");

				using var client = new HttpClient();
				var resp = await client.GetAsync(url);
				var content = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Scraping Error: GET request");
				throw;
			}
		}

		public async Task<string> GetHtmlSourceWithPlaywright(string url = Url) {
			try {
				_logger.LogDebug("Getting page source with Playwright");

				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Firefox.LaunchAsync(new() { Headless = true });

				var page = await browser.NewPageAsync();

				await page.GotoAsync(url);
				await page.WaitForLoadStateAsync(LoadState.Load);

				var content = await page.InnerHTMLAsync("*");

				_logger.LogDebug("Done");
				return content;
			} catch (Exception) {
				_logger.LogError("Scraping Error: Playwright");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
