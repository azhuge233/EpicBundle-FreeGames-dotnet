using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;

namespace EpicBundle_FreeGames_dotnet {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
		private static readonly IConfigurationRoot config = new ConfigurationBuilder()
				   .SetBasePath(System.IO.Directory.GetCurrentDirectory())
				   .Build();
		private static readonly string URL = "https://www.epicbundle.com/category/article/for-free/";

		public static IServiceProvider BuildDi() {
			return new ServiceCollection()
				.AddTransient<Scraper>()
				.AddTransient<Parser>()
				.AddTransient<TgBot>()
				.AddTransient<JsonOP>()
				.AddLogging(loggingBuilder => {
					// configure Logging with NLog
					loggingBuilder.ClearProviders();
					loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
					loggingBuilder.AddNLog(config);
				})
			   .BuildServiceProvider();
		}

		static async Task Main() {
			try {
				var servicesProvider = BuildDi();

				logger.Info(" - Start Job -");

				using (servicesProvider as IDisposable) {
					// Get telegram bot token, chatID and previous records
					var jsonOp = servicesProvider.GetRequiredService<JsonOP>();
					var tgConfig = jsonOp.LoadConfig(); // token, chatID
					var oldRecords = jsonOp.LoadData(); // old records

					// Get page source
					var scraper = servicesProvider.GetRequiredService<Scraper>();
					var source = scraper.GetHtmlSource(URL);

					// Parse page source
					var parser = servicesProvider.GetRequiredService<Parser>();
					var parseResult = parser.Parse(source, oldRecords);
					var pushList = parseResult.Item1; // notification list
					var recordList = parseResult.Item2; // new records list

					// Write new records
					jsonOp.WriteData(recordList);

					//Send notifications
					var tgBot = servicesProvider.GetRequiredService<TgBot>();
					await tgBot.SendMessage(token: tgConfig["TOKEN"], chatID: tgConfig["CHAT_ID"], pushList, htmlMode: true);
				}

				logger.Info(" - Job End -\n\n");
			} catch (Exception ex) {
				logger.Error("{0}\n\n", ex.Message);
			} finally {
				LogManager.Shutdown();
			}
		}
	}
}
