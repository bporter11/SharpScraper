using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpScraper
{
	public static class WebUtils
	{
		public static IEnumerable<HtmlNode> FindAllApplicableHtmlNodes(HtmlNode? parent, Predicate<HtmlNode> predicate)
		{
			if (parent is null || predicate is null)
			{
				yield break;
			}

			if (predicate(parent))
			{
				yield return parent;
			}

			foreach (var child in parent.ChildNodes)
			{
				foreach (var selection in WebUtils.FindAllApplicableHtmlNodes(child, predicate))
				{
					yield return selection;
				}
			}
		}

		public static HtmlNode? FindHtmlNodeWithAttributeRecursive(HtmlNode? parent, string name, string attrib, string value)
		{
			if (parent is null)
			{
				return null;
			}

			if (name == parent.Name)
			{
				var attribute = parent.GetAttributeValue<string>(attrib, null!);

				if (attribute == value)
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

		public static HtmlNode? FindHtmlNodeWithIDRecursive(HtmlNode? parent, string name, string id)
		{
			if (parent is null)
			{
				return null;
			}

			if (parent.Id == id)
			{
				return parent;
			}

			foreach (var child in parent.ChildNodes)
			{
				var recurse = WebUtils.FindHtmlNodeWithIDRecursive(child, name, id);

				if (recurse is not null)
				{
					return recurse;
				}
			}

			return null;
		}

		public static HtmlNode? FindHtmlNodeWithNameRecursive(HtmlNode? parent, string name)
		{
			if (parent is null)
			{
				return null;
			}

			if (parent.Name == name)
			{
				return parent;
			}

			foreach (var child in parent.ChildNodes)
			{
				var recurse = WebUtils.FindHtmlNodeWithNameRecursive(child, name);

				if (recurse is not null)
				{
					return recurse;
				}
			}

			return null;
		}

		public static string GetHtmlNodeContentByPath(HtmlNode? parent, params string[] path)
		{
			if (parent is null || path.Length == 0)
			{
				return String.Empty;
			}

			string full;

			if (path.Length > 10)
			{
				var sb = new StringBuilder(parent.XPath);

				for (int i = 0; i < path.Length; ++i)
				{
					_ = sb.Append('/');
					_ = sb.Append(path[i]);
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

		public static async Task<HtmlDocument?> TryReceiveHtmlPageAsync(string? page)
		{
			if (String.IsNullOrWhiteSpace(page))
			{
				return null;
			}

			return await new HtmlWeb().LoadFromWebAsync(page);
		}
	}
}
