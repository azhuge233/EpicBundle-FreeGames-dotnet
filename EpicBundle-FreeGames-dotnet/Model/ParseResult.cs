using System.Collections.Generic;

namespace EpicBundle_FreeGames_dotnet.Model {
	public class ParseResult {
		public List<FreeGameRecord> PushList { get; set; }
		public List<FreeGameRecord> RecordList { get; set; }

		public ParseResult() {
			PushList = new List<FreeGameRecord>();
			RecordList = new List<FreeGameRecord>();
		}
	}
}
