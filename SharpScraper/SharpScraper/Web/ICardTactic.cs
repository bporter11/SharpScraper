using HtmlAgilityPack;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public interface ICardTactic
	{
		public string Name { get; }
		public double Price { get; }
		public string Rarity { get; }
		public string SetCode { get; }
		public string SetName { get; }

		public Task Parse(HtmlDocument document);
	}
}
