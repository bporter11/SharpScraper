using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class CardRushTest
	{
		private static async Task<CardRushTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<CardRushTactic>(CardRushTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[0] is CardRushTactic);

			return (factory.LoadedCards[0] as CardRushTactic)!;
		}

		[Theory]
		[InlineData("https://www.cardrush-pokemon.jp/product/3855", "ミュウツーEX")]
		[InlineData("https://www.cardrush-pokemon.jp/product/3850", "リザードンex")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData("https://www.cardrush-pokemon.jp/product/3855", 10800.0)]
		[InlineData("https://www.cardrush-pokemon.jp/product/3850", 32800.0)]
		public async Task PriceAsyncTest(string url, double price)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Price, price);
		}

		[Theory]
		[InlineData("https://www.cardrush-pokemon.jp/product/3855", "UR")]
		[InlineData("https://www.cardrush-pokemon.jp/product/3850", "-")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData("https://www.cardrush-pokemon.jp/product/3855", "065/059")]
		[InlineData("https://www.cardrush-pokemon.jp/product/3850", "012/052")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData("https://www.cardrush-pokemon.jp/product/3855", "バーストボール")]
		[InlineData("https://www.cardrush-pokemon.jp/product/3850", "")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}
}
