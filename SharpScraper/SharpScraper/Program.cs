using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SharpScraper
{
	class Program
	{
		private const int kFailedGetPageResponseCode = 1;
		private const int kFailedGetPageContentsCode = 2;

		public static void ParseUserInput(string url)
		{
			var result = Program.TryReceiveHtmlPage(url);

			while (!result.IsCompleted)
			{
				// #TODO: make waiting notification
			}

			if (result.Status != TaskStatus.RanToCompletion)
			{
				Console.WriteLine("Failed to get page response. Exiting...");
				Environment.Exit(Program.kFailedGetPageResponseCode);
			}

			var document = result.Result;

			if (document is null)
			{
				Console.WriteLine("Failed to get page contents. Exiting...");
				Environment.Exit(Program.kFailedGetPageContentsCode);
			}

			Debug.Assert(Program.TryGetHtmlNodeByNodeName(document, "main_container", out var container));

			var goods = Program.FindHtmlNodeWithAttributeRecursive(container, "class", "goods_name");
			var price = Program.FindHtmlNodeWithAttributeRecursive(container, "class", "selling_price");
			var model = Program.FindHtmlNodeWithAttributeRecursive(container, "class", "model_number_value");
			var stock = Program.FindHtmlNodeWithAttributeRecursive(container, "class", "detail_section stock soldout");

			Console.WriteLine($"URL: {url}");
			Console.WriteLine($"Goods found: [{goods is not null}]");
			Console.WriteLine($"Price found: [{price is not null}]");
			Console.WriteLine($"Model found: [{model is not null}]");
			Console.WriteLine($"Stock found: [{model is not null}]");
			Console.WriteLine("-----------------------------------");

			Debugger.Break();
		}

		public static async Task<HtmlDocument> TryReceiveHtmlPage(string page)
		{
			if (String.IsNullOrWhiteSpace(page))
			{
				return null;
			}

			return await new HtmlWeb().LoadFromWebAsync(page);
		}

		public static bool TryGetHtmlNodeByNodeName(HtmlDocument document, string name, out HtmlNode node)
		{
			node = document.GetElementbyId(name);
			return node is not null;
		}

		public static HtmlNode FindHtmlNodeWithAttributeRecursive(HtmlNode parent, string attrib, string value)
		{
			if (parent is null)
			{
				return null;
			}

			var attribute = parent.GetAttributeValue<string>(attrib, null);

			if (String.CompareOrdinal(attribute, value) == 0)
			{
				return parent;
			}

			foreach (var child in parent.ChildNodes)
			{
				var recurse = Program.FindHtmlNodeWithAttributeRecursive(child, attrib, value);

				if (recurse is not null)
				{
					return recurse;
				}
			}

			return null;
		}

		static void Main(string[] args)
		{
			args = new string[]
			{
				"https://www.cardrush-pokemon.jp/product/3855",
				"https://www.cardrush-pokemon.jp/product/3856",
			};

			if (args.Length == 0)
			{
				Console.WriteLine($"Number of arguments should be at least 1 (URL)");
			}

			foreach (var url in args)
			{
				Program.ParseUserInput(url);
			}

			Debugger.Break();
		}
	}
}
