using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EpicBundle_FreeGames_dotnet.Model;

namespace EpicBundle_FreeGames_dotnet.Notifier {
	interface INotifiable: IDisposable {
		public Task SendMessage(NotifyConfig coonfig, List<FreeGameRecord> records);
	}
}
