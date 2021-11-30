using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Implements <see cref="ICardTactic"/> that operates on CardRush domain.
	/// </summary>
	public class CardRushTactic : ICardTactic
	{
		private const string kNameRegEx = @"^(?<name>[^()]*)[(]*(?<setname>[^()]*)[)]*【(?<rarity>.*)】{(?<setcode>.*)}$";
		private const string kCostRegEx = @"(?<price>[\d.,]+)";

		private string m_name;
		private double m_price;
		private string m_rarity;
		private string m_setCode;
		private string m_setName;

		/// <summary>
		/// Domain that this class uses as a target.
		/// </summary>
		public static readonly string Domain = "www.cardrush-pokemon.jp";

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
		/// Constructs new instance of <see cref="CardRushTactic"/>.
		/// </summary>
		public CardRushTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_setName = String.Empty;
		}

		/// <inheritdoc/>
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
