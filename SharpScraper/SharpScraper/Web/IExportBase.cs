using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Provides basic functionality for exporting <see cref="ICardTactic"/> collection as serialized data.
	/// </summary>
	public interface IExportBase
	{
		/// <summary>
		/// Exports collection of <see cref="ICardTactic"/> by serializing them into <see cref="Stream"/> provided.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> to serialize into.</param>
		/// <param name="cards"><see cref="IDictionary{TKey, TValue}"/> of cards and URLs to export.</param>
		/// <returns><see cref="Task.CompletedTask"/> on successful compleition.</returns>
		public Task Export(Stream stream, IDictionary<string, ICardTactic> cards);
	}
}
