using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class TextExportBase : IExportBase
	{
		public static readonly string Name = "TXT";

		public TextExportBase()
		{
		}

		public Task Export(Stream stream, IDictionary<string, ICardTactic> cards)
		{
			var writer = new StreamWriter(stream);

			var table = new string[6, cards.Count + 1];
			var padds = new int[6];

			table[0, 0] = "Name";
			table[1, 0] = "Price";
			table[2, 0] = "Rarity";
			table[3, 0] = "Set Code";
			table[4, 0] = "Set Name";
			table[5, 0] = "URL";

			int counter = 1;
			int maxpads = 1;

			foreach (var pair in cards)
			{
				var url = pair.Key;
				var val = pair.Value;

				table[0, counter] = val.Name;
				table[1, counter] = val.Price.ToString();
				table[2, counter] = val.Rarity;
				table[3, counter] = val.SetCode;
				table[4, counter] = val.SetName;
				table[5, counter] = url;

				++counter;
			}

			for (int i = 0; i < 6; ++i)
			{
				for (int k = 0; k < cards.Count + 1; ++k)
				{
					padds[i] = Math.Max(padds[i], table[i, k].Length);
				}

				maxpads += 3 + padds[i];
			}

			var padString = new string('-', maxpads);

			for (int i = 0; i < cards.Count + 1; ++i)
			{
				writer.WriteLine(padString);

				for (int k = 0; k < 6; ++k)
				{
					var str = table[k, i];

					writer.Write($"| {str} ");

					for (int p = 0; p < padds[k] - str.Length; ++p)
					{
						writer.Write(' ');
					}
				}

				writer.WriteLine('|');
			}

			writer.WriteLine(padString);
			writer.Flush();

			return Task.CompletedTask;
		}
	}
}
