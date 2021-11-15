using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class TrollAndToadTactic : ICardTactic
	{
		private string m_name;
		private double m_price;
		private string m_rarity;
		private string m_setCode;
		private string m_setName;

		public static readonly string Domain = "www.trollandtoad.com";

		public string Name => this.m_name;
		public double Price => this.m_price;
		public string Rarity => this.m_rarity;
		public string SetCode => this.m_setCode;
		public string SetName => this.m_setName;

		public TrollAndToadTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_setName = String.Empty;
		}

		public Task Parse(HtmlDocument document)
		{
			var header = WebUtils.FindHtmlNodeWithIDRecursive(document.DocumentNode, "div", "prodContainer");
			var column = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "div", "class", "buyBox card shadow-sm");

			if (header is not null)
			{
				var row = WebUtils.FindHtmlNodeWithAttributeRecursive(header, "div", "class", "row");

				if (row is not null)
				{
					var titles = WebUtils.FindAllApplicableHtmlNodes(row, (node) =>
					{
						if (node.Name != "div")
						{
							return false;
						}

						if (node.GetAttributeValue("class", String.Empty) == "col-12")
						{
							return true;
						}
						else
						{
							return false;
						}
					}).ToArray();

					if (titles.Length > 0)
					{
						var major = WebUtils.FindHtmlNodeWithAttributeRecursive(titles[0], "h1", "itemprop", "name");

						if (major is not null)
						{
							var splits = major.InnerText.Trim().Split('-', StringSplitOptions.RemoveEmptyEntries);

							switch (splits.Length)
							{
								default:
									this.m_setCode = splits[1].Trim();
									goto case 2;

								case 2:
									this.m_rarity = splits[^1].Trim();
									goto case 1;

								case 1:
									this.m_name = splits[0].Trim();
									break;

								case 0:
									break;
							}
						}
					}

					if (titles.Length > 1)
					{
						var major = WebUtils.FindHtmlNodeWithNameRecursive(titles[1], "h2");

						if (major is not null)
						{
							this.m_setName = major.InnerText.Trim();
						}
					}
				}
			}

			if (column is not null)
			{
				var node = WebUtils.FindHtmlNodeWithAttributeRecursive(column, "div", "class", "d-flex flex-column");

				if (node is not null)
				{
					var price = WebUtils.FindAllApplicableHtmlNodes(node, _ => _.Name == "span").FirstOrDefault();

					if (price is not null)
					{
						var trimed = price.InnerText.Trim();
						var substr = trimed.StartsWith('$') ? trimed[1..] : trimed;

						_ = Double.TryParse(substr, out this.m_price);
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}
