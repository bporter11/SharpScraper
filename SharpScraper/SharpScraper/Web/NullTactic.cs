using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class NullTactic : ICardTactic
	{
		public bool IsNull => true;
		public string Name => throw new NotImplementedException();
		public double Price => throw new NotImplementedException();
		public string Rarity => throw new NotImplementedException();
		public string SetCode => throw new NotImplementedException();
		public string SetName => throw new NotImplementedException();

		public Task Parse(HtmlDocument document) => throw new NotImplementedException();
	}
}
