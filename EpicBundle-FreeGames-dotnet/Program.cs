using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using EpicBundle_FreeGames_dotnet.Module;

namespace EpicBundle_FreeGames_dotnet {
	class Program {
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		static async Task Main() {
			try {
				var servicesProvider = DI.BuildDiAll();

				logger.Info(" - Start Job -");

				using (servicesProvider as IDisposable) {
					var jsonOp = servicesProvider.GetRequiredService<JsonOP>();
					var config = jsonOp.LoadConfig();
					servicesProvider.GetRequiredService<ConfigValidator>().CheckValid(config);

					// Get page source
					var source = servicesProvider.GetRequiredService<Scraper>().GetHtmlSource();

					// Parse source
					var parseResult = servicesProvider.GetRequiredService<Parser>().Parse(source, jsonOp.LoadData());

					//Send notifications
					await servicesProvider.GetRequiredService<NotifyOP>().Notify(config, parseResult.PushList);

					// Write new records
					jsonOp.WriteData(parseResult.RecordList);
				}

				logger.Info(" - Job End -\n\n");
			} catch (Exception ex) {
				logger.Error(ex.Message);
				logger.Error($"{ex.InnerException.Message}\n\n");
			} finally {
				LogManager.Shutdown();
			}
		}
	}
}
