using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class CardCrushTactic : ICardTactic
	{
		private const string kNameRegEx = @"^(?<name>[^()]*)[(]?.*[)]?【(?<rarity>.*)】{(?<setcode>.*)}$";
		private const string kCostRegEx = @"^[^\d]*(?<price>[\d.,]*).*$";
		private const string kLeftRegEx = @"(?<in_stock>[\d]+)";

		private string m_name;
		private string m_rarity;
		private string m_setCode;

		private double m_price;
		private string m_model;

		private int m_inStock;
		private bool m_soldOut;

		public static string Domain { get; } = "www.cardrush-pokemon.jp";

		public string Name => this.m_name;

		public string Rarity => this.m_rarity;

		public string SetCode => this.m_setCode;

		public double Price => this.m_price;

		public string Model => this.m_model;

		public int InStock => this.m_inStock;

		public bool SoldOut => this.m_soldOut;

		public CardCrushTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_model = String.Empty;
		}

		public Task Parse(HtmlDocument document)
		{
			Debug.Assert(WebUtils.TryGetHtmlNodeByNodeName(document, "main_container", out var container));

			var goods = WebUtils.FindHtmlNodeWithAttributeRecursive(container, "class", "goods_name");
			var price = WebUtils.FindHtmlNodeWithAttributeRecursive(container, "class", "selling_price");
			var model = WebUtils.FindHtmlNodeWithAttributeRecursive(container, "class", "model_number_value");
			var stock = WebUtils.FindHtmlNodeWithAttributeRecursive(container, "class", "detail_section stock");
			var cross = WebUtils.FindHtmlNodeWithAttributeRecursive(container, "class", "detail_section stock soldout");

			if (goods is not null)
			{
				var token = new Regex(CardCrushTactic.kNameRegEx).Match(goods.InnerText.Trim());

				this.m_name = token.Groups["name"].ToString();
				this.m_rarity = token.Groups["rarity"].ToString();
				this.m_setCode = token.Groups["setcode"].ToString();
			}

			if (price is not null)
			{
				var token = new Regex(CardCrushTactic.kCostRegEx).Match(price.InnerText.Trim());

				if (Double.TryParse(token.Groups["price"].ToString(), out var result))
				{
					this.m_price = result;
				}
			}

			if (model is not null)
			{
				this.m_model = model.InnerText.Trim();
			}

			if (stock is not null)
			{
				var token = new Regex(CardCrushTactic.kLeftRegEx).Match(stock.InnerText.Trim());

				if (Int32.TryParse(token.Groups["in_stock"].ToString(), out int result))
				{
					this.m_inStock = result;
				}
			}

			if (cross is not null)
			{
				this.m_soldOut = cross.InnerHtml.Trim() == "×";
			}

			return Task.CompletedTask;
		}
	}
}
