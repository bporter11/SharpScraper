using HtmlAgilityPack;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Implements <see cref="ICardTactic"/> that operates on TCGMP domain.
	/// </summary>
	public class TCGMPTactic : ICardTactic
	{
		private string m_name;
		private double m_price;
		private string m_rarity;
		private string m_setCode;
		private string m_setName;

		/// <summary>
		/// Domain that this class uses as a target.
		/// </summary>
		public static readonly string Domain = "tcgmp.jp";

		/// <summary>
		/// Returns constant <see langword="false"/>.
		/// </summary>
		public bool IsNull => false;

		/// <inheritdoc/>
		public string Name => this.m_name;

		/// <inheritdoc/>
		public double Price => this.m_price;

		/// <inheritdoc/>
		public string Rarity => this.m_rarity;

		/// <inheritdoc/>
		public string SetCode => this.m_setCode;

		/// <inheritdoc/>
		public string SetName => this.m_setName;

		/// <summary>
		/// Constructs new instance of <see cref="TCGMPTactic"/>.
		/// </summary>
		public TCGMPTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_setName = String.Empty;
		}

		/// <inheritdoc/>
		public Task Parse(HtmlDocument document)
		{
			var goods = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "span", "class", "name");
			var datas = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "div", "class", "data");
			var table = WebUtils.FindHtmlNodeWithIDRecursive(document.DocumentNode, "div", "stocks_body");

			if (goods is not null)
			{
				this.m_name = goods.InnerText.Trim();
			}

			if (datas is not null)
			{
				this.m_setCode = WebUtils.GetHtmlNodeContentByPath(datas, "table[1]", "tbody[1]", "tr[1]", "td[1]").Trim().Split('/').RawConcat(0, 2, "/");
				this.m_setName = WebUtils.GetHtmlNodeContentByPath(datas, "table[1]", "tbody[1]", "tr[2]", "td[1]").Trim();
				this.m_rarity = WebUtils.GetHtmlNodeContentByPath(datas, "table[1]", "tbody[1]", "tr[3]", "td[1]").Trim();
			}

			if (table is not null)
			{
				var priceNodes = WebUtils.FindAllApplicableHtmlNodes(table, (node) =>
				{
					if (node.Name != "strong")
					{
						return false;
					}

					if (node.GetAttributeValue("class", String.Empty) == "price")
					{
						return node.InnerText.Trim().StartsWith('???');
					}
					else
					{
						return false;
					}
				}).ToArray();

				var priceLists = new double[priceNodes.Length];

				for (int i = 0; i < priceLists.Length; ++i)
				{
					var innerText = priceNodes[i].InnerText.Trim()[1..].Trim();

					priceLists[i] = Double.TryParse(innerText, out var price) ? price : Double.PositiveInfinity;
				}

				this.m_price = priceLists.Min();
			}

			return Task.CompletedTask;
		}
	}
}
