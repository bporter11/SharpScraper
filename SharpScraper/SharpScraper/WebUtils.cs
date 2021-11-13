using HtmlAgilityPack;

using System;
using System.Text;
using System.Threading.Tasks;

namespace SharpScraper
{
	public static class WebUtils
	{
		public static HtmlNode FindHtmlNodeWithAttributeRecursive(HtmlNode parent, string name, string attrib, string value)
		{
			if (parent is null)
			{
				return null;
			}

			if (String.CompareOrdinal(name, parent.Name) == 0)
			{
				var attribute = parent.GetAttributeValue<string>(attrib, null);

				if (String.CompareOrdinal(attribute, value) == 0)
				{
					return parent;
				}
			}

			foreach (var child in parent.ChildNodes)
			{
				var recurse = WebUtils.FindHtmlNodeWithAttributeRecursive(child, name, attrib, value);

				if (recurse is not null)
				{
					return recurse;
				}
			}

			return null;
		}

		public static string GetHtmlNodeContentByPath(HtmlNode parent, params string[] path)
		{
			if (parent is null || path.Length == 0)
			{
				return String.Empty;
			}

			string full = String.Empty;

			if (path.Length > 10)
			{
				var sb = new StringBuilder(parent.XPath);

				for (int i = 0; i < path.Length; ++i)
				{
					sb.Append('/');
					sb.Append(path[i]);
				}

				full = sb.ToString();
			}
			else
			{
				full = parent.XPath;

				for (int i = 0; i < path.Length; ++i)
				{
					full += '/' + path[i];
				}
			}

			var navigator = parent.CreateRootNavigator();
			return navigator.SelectSingleNode(full)?.Value ?? String.Empty;
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
