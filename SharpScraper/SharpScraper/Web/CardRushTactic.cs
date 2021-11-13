using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class CardRushTactic : ICardTactic
	{
		private const string kNameRegEx = @"^(?<name>[^()]*)[(]*(?<setname>[^()]*)[)]*【(?<rarity>.*)】{(?<setcode>.*)}$";
		private const string kCostRegEx = @"(?<price>[\d.,]+)";

		private string m_name;
		private double m_price;
		private string m_rarity;
		private string m_setCode;
		private string m_setName;

		public static readonly string Domain = "www.cardrush-pokemon.jp";

		public string Name => this.m_name;
		public double Price => this.m_price;
		public string Rarity => this.m_rarity;
		public string SetCode => this.m_setCode;
		public string SetName => this.m_setName;

		public CardRushTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_setName = String.Empty;
		}

		public Task Parse(HtmlDocument document)
		{
			var goods = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "span", "class", "goods_name");
			var price = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "span", "class", "figure");

			if (goods is not null)
			{
				var token = new Regex(CardRushTactic.kNameRegEx).Match(goods.InnerText.Trim());

				this.m_name = token.Groups["name"].ToString();
				this.m_rarity = token.Groups["rarity"].ToString();
				this.m_setCode = token.Groups["setcode"].ToString();
				this.m_setName = token.Groups["setname"].ToString();
			}

			if (price is not null)
			{
				var token = new Regex(CardRushTactic.kCostRegEx).Match(price.InnerText.Trim());

				_ = Double.TryParse(token.Groups["price"].ToString(), out this.m_price);
			}

			return Task.CompletedTask;
		}
	}
}
