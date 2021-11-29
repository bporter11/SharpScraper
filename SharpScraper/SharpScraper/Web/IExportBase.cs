using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public interface IExportBase
	{
		public Task Export(Stream stream, IDictionary<string, ICardTactic> cards);
	}
}
