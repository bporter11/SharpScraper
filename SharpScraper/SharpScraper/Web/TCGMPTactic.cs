using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class TCGMPTactic : ICardTactic
	{
		private string m_name;
		private string m_rarity;
		private string m_setCode;

		private double m_price;
		private string m_model;

		private int m_inStock;
		private bool m_soldOut;

		public static string Domain { get; } = "tcgmp.jp";

		public string Name => this.m_name;

		public string Rarity => this.m_rarity;

		public string SetCode => this.m_setCode;

		public double Price => this.m_price;

		public string Model => this.m_model;

		public int InStock => this.m_inStock;

		public bool SoldOut => this.m_soldOut;

		public TCGMPTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_model = String.Empty;
		}

		public Task Parse(HtmlDocument document)
		{
			// #TODO

			return Task.CompletedTask;
		}
	}
}
