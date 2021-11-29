using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class CardRushTest
	{
		private const string kWebTest1 = "https://www.cardrush-pokemon.jp/product/3855";
		private const string kWebTest2 = "https://www.cardrush-pokemon.jp/product/3850";
		private const string kWebTest3 = "https://www.cardrush-pokemon.jp/product/3840";

		private static async Task<CardRushTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<CardRushTactic>(CardRushTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[url] is CardRushTactic);

			return (factory.LoadedCards[url] as CardRushTactic)!;
		}

		[Theory]
		[InlineData(CardRushTest.kWebTest1, "ミュウツーEX")]
		[InlineData(CardRushTest.kWebTest2, "リザードンex")]
		[InlineData(CardRushTest.kWebTest3, "リザードンEX")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData(CardRushTest.kWebTest1, 10800.0)]
		[InlineData(CardRushTest.kWebTest2, 32800.0)]
		[InlineData(CardRushTest.kWebTest3, 59800.0)]
		public async Task PriceAsyncTest(string url, double price)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Price, price);
		}

		[Theory]
		[InlineData(CardRushTest.kWebTest1, "UR")]
		[InlineData(CardRushTest.kWebTest2, "-")]
		[InlineData(CardRushTest.kWebTest3, "SR")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData(CardRushTest.kWebTest1, "065/059")]
		[InlineData(CardRushTest.kWebTest2, "012/052")]
		[InlineData(CardRushTest.kWebTest3, "090/087")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData(CardRushTest.kWebTest1, "バーストボール")]
		[InlineData(CardRushTest.kWebTest2, "")]
		[InlineData(CardRushTest.kWebTest3, "")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}

	public class CardRushFactoryTest : CardFactoryTest<CardRushTactic>
	{
		public override string Domain => CardRushTactic.Domain;
	}
}
