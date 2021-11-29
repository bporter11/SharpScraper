using SharpScraper.Web;
using Xunit;

namespace ScraperXUnit
{
	public abstract class CardFactoryTest<T> where T : ICardTactic, new()
	{
		public abstract string Domain { get; }

		[Fact]
		public void RegisterTest()
		{
			var factory = new CardFactory();

			factory.RegisterTactic<T>(this.Domain);

			Assert.True(factory.IsTacticRegistered(this.Domain));
		}
	}
}
