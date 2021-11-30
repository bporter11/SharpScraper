using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpScraper
{
	/// <summary>
	/// Provides utilities for recursive <see cref="HtmlNode"/> iterations and lookups.
	/// </summary>
	public static class WebUtils
	{
		/// <summary>
		/// Finds all child <see cref="HtmlNode"/> that match <see cref="Predicate{T}"/> passed.
		/// </summary>
		/// <param name="parent">Parent <see cref="HtmlNode"/> on which iteration should occur.</param>
		/// <param name="predicate"><see cref="Predicate{T}"/> that indicates pattern of node selections.</param>
		/// <returns><see cref="IEnumerable{T}"/> of all <see cref="HtmlNode"/> that match <see cref="Predicate{T}"/> passed.</returns>
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

		/// <summary>
		/// Recursively searches for <see cref="HtmlNode"/> based on arguments passed.
		/// </summary>
		/// <param name="parent">Parent <see cref="HtmlNode"/> to start recursive search from.</param>
		/// <param name="name">Name of the node to search for.</param>
		/// <param name="attrib">Attribute name of the node to search for.</param>
		/// <param name="value">Value of the attribute to search for.</param>
		/// <returns><see cref="HtmlNode"/> that matches arguments passed, if found; otherwise, <see langword="null"/>.</returns>
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

		/// <summary>
		/// Recursively searches for <see cref="HtmlNode"/> based on arguments passed.
		/// </summary>
		/// <param name="parent">Parent <see cref="HtmlNode"/> to start recursive search from.</param>
		/// <param name="name">Name of the node to search for.</param>
		/// <param name="id">ID value of the node to search for.</param>
		/// <returns><see cref="HtmlNode"/> that matches arguments passed, if found; otherwise, <see langword="null"/>.</returns>
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

		/// <summary>
		/// Recursively searches for <see cref="HtmlNode"/> based on arguments passed.
		/// </summary>
		/// <param name="parent">Parent <see cref="HtmlNode"/> to start recursive search from.</param>
		/// <param name="name">Name of the node to search for.</param>
		/// <returns><see cref="HtmlNode"/> that matches arguments passed, if found; otherwise, <see langword="null"/>.</returns>
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

		/// <summary>
		/// Gets content of <see cref="HtmlNode"/> using XML path provided.
		/// </summary>
		/// <param name="parent">Parent <see cref="HtmlNode"/> to start path finding from.</param>
		/// <param name="path">Path arguments of the final node to look for.</param>
		/// <returns>Content of <see cref="HtmlNode"/>, if found using path provided; otherwise, <see cref="String.Empty"/>.</returns>
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

		/// <summary>
		/// Attempts to asynchronously get <see cref="HtmlDocument"/> using URL provided.
		/// </summary>
		/// <param name="page">URL of the page to get data from.</param>
		/// <returns><see cref="HtmlDocument"/> on successful operation; otherwise, <see langword="null"/>.</returns>
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
