using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class TrollAndToadTest
	{
		private const string kWebTest1 = "https://www.trollandtoad.com/pokemon/pokemon-xy-promos/pikachu-ex-xy124-ultra-rare-promo/1101790";
		private const string kWebTest2 = "https://www.trollandtoad.com/pokemon/sword-shield-rebel-clash-singles/capture-energy-171-192-uncommon/1664128";
		private const string kWebTest3 = "https://www.trollandtoad.com/pokemon/pokemon-theme-deck-exclusives/marnie-169-202-rare-theme-deck-exclusive/1654641";

		private static async Task<TrollAndToadTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<TrollAndToadTactic>(TrollAndToadTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[url] is TrollAndToadTactic);

			return (factory.LoadedCards[url] as TrollAndToadTactic)!;
		}

		[Theory]
		[InlineData(TrollAndToadTest.kWebTest1, "Pikachu EX")]
		[InlineData(TrollAndToadTest.kWebTest2, "Capture Energy")]
		[InlineData(TrollAndToadTest.kWebTest3, "Marnie")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData(TrollAndToadTest.kWebTest1)]
		[InlineData(TrollAndToadTest.kWebTest2)]
		[InlineData(TrollAndToadTest.kWebTest3)]
		public async Task PriceAsyncTest(string url)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.NotEqual(0.0, loadedCard.Price);
		}

		[Theory]
		[InlineData(TrollAndToadTest.kWebTest1, "Ultra Rare Promo")]
		[InlineData(TrollAndToadTest.kWebTest2, "Uncommon")]
		[InlineData(TrollAndToadTest.kWebTest3, "Rare Theme Deck Exclusive")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData(TrollAndToadTest.kWebTest1, "XY124")]
		[InlineData(TrollAndToadTest.kWebTest2, "171/192")]
		[InlineData(TrollAndToadTest.kWebTest3, "169/202")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData(TrollAndToadTest.kWebTest1, "Pokemon XY Promos")]
		[InlineData(TrollAndToadTest.kWebTest2, "Sword & Shield: Rebel Clash Singles")]
		[InlineData(TrollAndToadTest.kWebTest3, "Pokemon Theme Deck Exclusives")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}

	public class TrollAndToadFactoryTest : CardFactoryTest<TrollAndToadTactic>
	{
		public override string Domain => TrollAndToadTactic.Domain;
	}
}
