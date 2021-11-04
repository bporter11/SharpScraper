using HtmlAgilityPack;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public interface ICardTactic
	{
		public string Name { get; }
		public string Rarity { get; }
		public string SetCode { get; }

		public double Price { get; }
		public bool InStock { get; }
		public bool SoldOut { get; }

		public Task Parse(HtmlDocument document);
	}
}
