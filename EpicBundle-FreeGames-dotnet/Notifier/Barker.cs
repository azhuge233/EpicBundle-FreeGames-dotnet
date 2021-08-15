using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using EpicBundle_FreeGames_dotnet.Model;

namespace EpicBundle_FreeGames_dotnet.Notifier {
	class Barker : INotifiable {
		private readonly ILogger<Barker> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Bark";
		#endregion

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				var sb = new StringBuilder();
				string url = sb.AppendFormat(NotifyFormatStrings.barkUrlFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();

				foreach (var record in records) {
					sb.Clear();
					_logger.LogDebug($"{debugSendMessage} : {record.Title}");
					await webGet.LoadFromWebAsync(
						sb.Append(url)
						.Append(NotifyFormatStrings.barkUrlTitle)
						.Append(HttpUtility.UrlEncode(record.ToBarkMessage()))
						.AppendFormat(NotifyFormatStrings.barkUrlArgs, record.PossibleLinks.FirstOrDefault(), record.PossibleLinks.FirstOrDefault())
						.ToString()
					);
				}

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {debugSendMessage}");
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
