using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class CardMarketTest
	{
		private const string kWebTest1 = "https://www.cardmarket.com/en/Pokemon/Products/Singles/XY-Black-Star-Promos/Pikachu-EX-V1-XYPRXY124#articleFilterProductLanguage";
		private const string kWebTest2 = "https://www.cardmarket.com/en/YuGiOh/Products/Singles/Duelist-Pack-Kaiba/Pot-of-Greed";
		private const string kWebTest3 = "https://www.cardmarket.com/en/Pokemon/Products/Singles/Celebrations/Reshiram-V1-CEL002";

		private static async Task<CardMarketTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<CardMarketTactic>(CardMarketTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[url] is CardMarketTactic);

			return (factory.LoadedCards[url] as CardMarketTactic)!;
		}

		[Theory]
		[InlineData(CardMarketTest.kWebTest1, "Pikachu EX")]
		[InlineData(CardMarketTest.kWebTest2, "Pot of Greed")]
		[InlineData(CardMarketTest.kWebTest3, "Reshiram")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData(CardMarketTest.kWebTest1)]
		[InlineData(CardMarketTest.kWebTest2)]
		[InlineData(CardMarketTest.kWebTest3)]
		public async Task PriceAsyncTest(string url)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.NotEqual(0.0, loadedCard.Price);
		}

		[Theory]
		[InlineData(CardMarketTest.kWebTest1, "Promo")]
		[InlineData(CardMarketTest.kWebTest2, "Ultimate Rare")]
		[InlineData(CardMarketTest.kWebTest3, "Holo Rare")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData(CardMarketTest.kWebTest1, "XYPR 124")]
		[InlineData(CardMarketTest.kWebTest2, "")]
		[InlineData(CardMarketTest.kWebTest3, "CEL 002")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData(CardMarketTest.kWebTest1, "XY Black Star Promos - Singles")]
		[InlineData(CardMarketTest.kWebTest2, "Duelist Pack: Kaiba - Singles")]
		[InlineData(CardMarketTest.kWebTest3, "Celebrations - Singles")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}

	public class CardMarketFactoryTest : CardFactoryTest<CardMarketTactic>
	{
		public override string Domain => CardMarketTactic.Domain;
	}
}
