using SharpScraper.Web;
using System.Threading.Tasks;
using Xunit;

namespace ScraperXUnit
{
	public class PokemonWizardTest
	{
		private const string kWebTest1 = "https://www.pokemonwizard.com/cards/123436/pikachu-ex-xy124";
		private const string kWebTest2 = "https://www.pokemonwizard.com/cards/226434/pikachu-vmax-secret";
		private const string kWebTest3 = "https://www.pokemonwizard.com/cards/88109/pikachu-delta-species";

		private static async Task<PokemonWizardTactic> LoadURLAsync(string url)
		{
			var factory = new CardFactory();

			factory.RegisterTactic<PokemonWizardTactic>(PokemonWizardTactic.Domain);

			await factory.ParseAsync(url);

			Assert.True(factory.LoadedCards.Count == 1);
			Assert.True(factory.LoadedCards[url] is PokemonWizardTactic);

			return (factory.LoadedCards[url] as PokemonWizardTactic)!;
		}

		[Theory]
		[InlineData(PokemonWizardTest.kWebTest1, "Pikachu EX")]
		[InlineData(PokemonWizardTest.kWebTest2, "Pikachu VMAX")]
		[InlineData(PokemonWizardTest.kWebTest3, "Pikachu Delta Species")]
		public async Task NameAsyncTest(string url, string name)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Name, name);
		}

		[Theory]
		[InlineData(PokemonWizardTest.kWebTest1, 60.3)]
		[InlineData(PokemonWizardTest.kWebTest2, 2.8)]
		[InlineData(PokemonWizardTest.kWebTest3, 74.75)]
		public async Task PriceAsyncTest(string url, double price)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Price, price);
		}

		[Theory]
		[InlineData(PokemonWizardTest.kWebTest1, "Promo")]
		[InlineData(PokemonWizardTest.kWebTest2, "Secret Rare")]
		[InlineData(PokemonWizardTest.kWebTest3, "Holo Rare")]
		public async Task RarityAsyncTest(string url, string rarity)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.Rarity, rarity);
		}

		[Theory]
		[InlineData(PokemonWizardTest.kWebTest1, "XY124")]
		[InlineData(PokemonWizardTest.kWebTest2, "188")]
		[InlineData(PokemonWizardTest.kWebTest3, "35")]
		public async Task SetCodeAsyncTest(string url, string setCode)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetCode, setCode);
		}

		[Theory]
		[InlineData(PokemonWizardTest.kWebTest1, "XY Promos")]
		[InlineData(PokemonWizardTest.kWebTest2, "Vivid Voltage")]
		[InlineData(PokemonWizardTest.kWebTest3, "Nintendo Promos")]
		public async Task SetNameAsyncTest(string url, string setName)
		{
			var loadedCard = await LoadURLAsync(url);

			Assert.Equal(loadedCard.SetName, setName);
		}
	}

	public class PokemonWizardFactoryTest : CardFactoryTest<PokemonWizardTactic>
	{
		public override string Domain => PokemonWizardTactic.Domain;
	}
}
