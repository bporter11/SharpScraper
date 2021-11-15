using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class TCGMPTest
	{
		private static async Task<TCGMPTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<TCGMPTactic>(TCGMPTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[0] is TCGMPTactic);

			return (factory.LoadedCards[0] as TCGMPTactic)!;
		}

		[Theory]
		[InlineData("https://tcgmp.jp/product/detail?id=203727", "ピカチュウEX")]
		[InlineData("https://tcgmp.jp/product/detail?id=224743", "ひかるルギア")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData("https://tcgmp.jp/product/detail?id=203727", 60000.0)]
		[InlineData("https://tcgmp.jp/product/detail?id=224743", 7080.0)]
		public async Task PriceAsyncTest(string url, double price)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Price, price);
		}

		[Theory]
		[InlineData("https://tcgmp.jp/product/detail?id=203727", "ＳＲ")]
		[InlineData("https://tcgmp.jp/product/detail?id=224743", "☆")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData("https://tcgmp.jp/product/detail?id=203727", "094/087")]
		[InlineData("https://tcgmp.jp/product/detail?id=224743", "058/072")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData("https://tcgmp.jp/product/detail?id=203727", "コンセプトパック「20th Anniversary」")]
		[InlineData("https://tcgmp.jp/product/detail?id=224743", "強化拡張パック｢ひかる伝説｣")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}
}
