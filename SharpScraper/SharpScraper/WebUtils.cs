using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace SharpScraper
{
	public static class WebUtils
	{
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
				var recurse = WebUtils.FindHtmlNodeWithAttributeRecursive(child, attrib, value);

				if (recurse is not null)
				{
					return recurse;
				}
			}

			return null;
		}

		public static bool TryGetHtmlNodeByNodeName(HtmlDocument document, string name, out HtmlNode node)
		{
			node = document.GetElementbyId(name);
			return node is not null;
		}

		public static async Task<HtmlDocument> TryReceiveHtmlPage(string page)
		{
			if (String.IsNullOrWhiteSpace(page))
			{
				return null;
			}

			return await new HtmlWeb().LoadFromWebAsync(page);
		}
	}
}
