using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using System.Net;

namespace EpicBundle_FreeGames_dotnet {
	class Program {
		#region DI varialbles
		private readonly ILogger _logger;
		#endregion

		#region static varialbles
		private readonly string URL = "https://www.epicbundle.com/category/article/for-free/";
		private readonly string configPath = "config.json";
		private readonly string recordPath = "record.json";
		private readonly List<string> wordList = new List<string> { 
			"humble choice bundle",
			"follow us on facebook", 
			"more games for free here", 
			"\"limitless $10 epic coupons\"", 
			"add your comment." 
		};
		#endregion

		public Program(ILogger<Program> logger) {
			_logger = logger;
		}

		private static void ConfigureServices(ServiceCollection service) {
			service.AddLogging(logging => {
				logging.AddFilter("System", LogLevel.Warning);
				logging.AddFilter("Microsoft", LogLevel.Warning);
				logging.AddConsole();
			}).AddTransient<Program>();
		}

		internal async Task SendNotification(string id, string token, List<string> msgs) {
			if (msgs.Count == 0) {
				_logger.LogDebug("No new notifications !");
				return;
			}

			using var myBot = new TgBot(token);
			int count = 1;
			foreach (var msg in msgs) {
				_logger.LogDebug("Sending Message {0}", count++);
				await myBot.SendMessage(
					chatId: id,
					msg: msg,
					htmlMode: true
				);
			}
		}

		internal List<string> TryGetLinks(string url) {
			var results = new List<string>();

			#region getting all possible links
			_logger.LogDebug("Getting article page source");
			var webGet = new HtmlWeb();
			var htmlDoc = webGet.Load(url);
			_logger.LogDebug("Debug");

			var links = htmlDoc.DocumentNode.CssSelect("div.entry-content a");
			#endregion

			#region add links to list
			foreach (var each in links) {
				if (!wordList.Exists(x => x == each.InnerText.ToString().ToLower())) {
					string link = each.Attributes["href"].Value.ToString().Split('?')[0];

					if (link.Contains("#disqus_thread"))
						continue;

					_logger.LogInformation("Get possible link: {0}", link);
					results.Add(link);
				}
			}
			#endregion

			return results;
		} 

		internal async Task ProcessingData(HtmlDocument htmlDoc, List<Dictionary<string, string>> records, string chatId, string token) {
			var pushList = new List<string>();
			var recordList = new List<Dictionary<string, string>>();

			var articles = htmlDoc.DocumentNode.CssSelect("div.post-column article header h2 a");

			foreach (var each in articles) {
				#region get article titles and links
				var title = each.InnerText.ToString();
				var link = each.Attributes["href"].Value.ToString();

				_logger.LogInformation("Found new info: {0}", title);
				#endregion

				#region save titles and links to List
				var tmp = new Dictionary<string, string> {
					{ "title", title },
					{ "url", link}
				};
				recordList.Add(tmp);
				#endregion

				#region decide if push or not
				bool isPush = true;
				foreach (var i in records)
					if (i["title"] == title && i["url"] == link)
						isPush = false;
				#endregion

				#region push code
				if (isPush) {
					_logger.LogInformation("Add {0} to push list", title);

					//try to get links
					var possibleLinks = TryGetLinks(url: link);

					var tmpstr = "<b>EpicBundle 信息</b>\n\n";
					tmpstr += "<i>" + title + "</i>\n";
					tmpstr += "文章链接: " + link + "\n";
					tmpstr += "可能的领取链接:\n";

					foreach (var i in possibleLinks)
						tmpstr += i + "\n";

					pushList.Add(tmpstr);
				}
				#endregion
			}

			#region send notifications
			_logger.LogInformation("Sending notifications");
			await SendNotification(id: chatId, token: token, msgs: pushList);
			_logger.LogInformation("Done");
			#endregion

			#region writing records
			_logger.LogInformation("Writing records");
			using var jsonop = new JsonOP();
			try {
				jsonop.WriteData(data: recordList, path: recordPath);
			} catch (Exception ex) {
				_logger.LogError("Writing records failed!");
				_logger.LogError("Error message: {0}", ex.Message);
			}
			#endregion

		}

		internal async Task Run() {
			_logger.LogInformation(DateTime.Now.ToString() + " - Start Job - ");

			var records = new List<Dictionary<string, string>>();
			var config = new Dictionary<string, string>();

			#region load data
			using (var jsonop = new JsonOP()) {
				_logger.LogInformation("Loading previous records");
				try {
					records = jsonop.LoadData(recordPath);
				} catch (Exception ex) {
					_logger.LogError("Error loading previous records!");
					_logger.LogError("Error message: {0}", ex.Message);
				}
				_logger.LogInformation("Done");

				_logger.LogInformation("Loading configs");
				try {
					config = jsonop.LoadConfig(configPath);
				} catch (Exception ex) {
					_logger.LogError("Error loading configs!");
					_logger.LogError("Error message: {0}", ex.Message);
				}
				_logger.LogInformation("Done");
			}
			#endregion

			_logger.LogInformation("Getting page source");
			var webGet = new HtmlWeb();
			var htmlDoc = webGet.Load(URL);
			_logger.LogInformation("Done");

			_logger.LogInformation("Start processing data");
			await ProcessingData(htmlDoc: htmlDoc, records: records, chatId: config["CHAT_ID"], token: config["TOKEN"]);
			_logger.LogInformation("Done");

			_logger.LogInformation(DateTime.Now.ToString() + " - End Job - ");
		}

		static async Task Main() {
			var services = new ServiceCollection();
			ConfigureServices(services);
			using ServiceProvider serviceProvider = services.BuildServiceProvider();
			Program app = serviceProvider.GetService<Program>();
			await app.Run();
		}
	}
}
