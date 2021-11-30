using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Implements <see cref="IExportBase"/> that operates on .csv extension files.
	/// </summary>
	public class CSVExportBase : IExportBase
	{
		/// <summary>
		/// String identifier for this <see cref="IExportBase"/>.
		/// </summary>
		public static readonly string Name = "CSV";

		/// <inheritdoc/>
		public Task Export(Stream stream, IDictionary<string, ICardTactic> cards)
		{
			var writer = new StreamWriter(stream);

			WriteString("Name", true);
			WriteString("Price", true);
			WriteString("Rarity", true);
			WriteString("Set Code", true);
			WriteString("Set Name", true);
			WriteString("URL", false);
			writer.WriteLine();

			foreach (var pair in cards)
			{
				WriteString(pair.Value.Name, true);
				WriteString(pair.Value.Price.ToString(), true);
				WriteString(pair.Value.Rarity, true);
				WriteString(pair.Value.SetCode, true);
				WriteString(pair.Value.SetName, true);
				WriteString(pair.Key, false);
				writer.WriteLine();
			}

			writer.Flush();

			return Task.CompletedTask;

			void WriteString(string value, bool delim)
			{
				value = value.Replace("\"", "\"\"");

				if (value.Contains(',') || value.Contains('\"'))
				{
					value = "\"" + value + "\"";
				}

				writer.Write(value);

				if (delim)
				{
					writer.Write(',');
				}
			}
		}
	}
}
