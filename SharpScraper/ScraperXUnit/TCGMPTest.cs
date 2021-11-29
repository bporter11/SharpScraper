using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class TCGMPTest
	{
		private const string kWebTest1 = "https://tcgmp.jp/product/detail?id=203727";
		private const string kWebTest2 = "https://tcgmp.jp/product/detail?id=224743";
		private const string kWebTest3 = "https://tcgmp.jp/product/detail?id=224699";

		private static async Task<TCGMPTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<TCGMPTactic>(TCGMPTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[url] is TCGMPTactic);

			return (factory.LoadedCards[url] as TCGMPTactic)!;
		}

		[Theory]
		[InlineData(TCGMPTest.kWebTest1, "ピカチュウEX")]
		[InlineData(TCGMPTest.kWebTest2, "ひかるルギア")]
		[InlineData(TCGMPTest.kWebTest3, "ひかるゲノセクト")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData(TCGMPTest.kWebTest1, 60000.0)]
		[InlineData(TCGMPTest.kWebTest2, 6500.0)]
		[InlineData(TCGMPTest.kWebTest3, 1400.0)]
		public async Task PriceAsyncTest(string url, double price)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Price, price);
		}

		[Theory]
		[InlineData(TCGMPTest.kWebTest1, "ＳＲ")]
		[InlineData(TCGMPTest.kWebTest2, "☆")]
		[InlineData(TCGMPTest.kWebTest3, "☆")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData(TCGMPTest.kWebTest1, "094/087")]
		[InlineData(TCGMPTest.kWebTest2, "058/072")]
		[InlineData(TCGMPTest.kWebTest3, "010/072")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData(TCGMPTest.kWebTest1, "コンセプトパック「20th Anniversary」")]
		[InlineData(TCGMPTest.kWebTest2, "強化拡張パック｢ひかる伝説｣")]
		[InlineData(TCGMPTest.kWebTest3, "強化拡張パック｢ひかる伝説｣")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}

	public class TCGMPFactoryTest : CardFactoryTest<TCGMPTactic>
	{
		public override string Domain => TCGMPTactic.Domain;
	}
}
