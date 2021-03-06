using HtmlAgilityPack;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Implements <see cref="ICardTactic"/> that operates on PokemonWizard domain.
	/// </summary>
	public class PokemonWizardTactic : ICardTactic
	{
		private string m_name;
		private double m_price;
		private string m_rarity;
		private string m_setCode;
		private string m_setName;

		/// <summary>
		/// Domain that this class uses as a target.
		/// </summary>
		public static readonly string Domain = "www.pokemonwizard.com";

		/// <inheritdoc/>
		public bool IsNull => false;

		/// <summary>
		/// Returns constant <see langword="false"/>.
		/// </summary>
		public string Name => this.m_name;

		/// <inheritdoc/>
		public double Price => this.m_price;

		/// <inheritdoc/>
		public string Rarity => this.m_rarity;

		/// <inheritdoc/>
		public string SetCode => this.m_setCode;

		/// <inheritdoc/>
		public string SetName => this.m_setName;

		/// <summary>
		/// Constructs new instance of <see cref="PokemonWizardTactic"/>.
		/// </summary>
		public PokemonWizardTactic()
		{
			this.m_name = String.Empty;
			this.m_rarity = String.Empty;
			this.m_setCode = String.Empty;
			this.m_setName = String.Empty;
		}

		/// <inheritdoc/>
		public Task Parse(HtmlDocument document)
		{
			var names = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "h1", "class", "h1-header");
			var table = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "table", "class", "table table-borderless table-sm");
			var price = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "span", "class", "text-danger");

			if (names is not null)
			{
				this.m_name = names.InnerText.Trim();
			}

			if (table is not null)
			{
				var pathy = WebUtils.FindHtmlNodeWithNameRecursive(table, "tbody");

				if (pathy is not null)
				{
					var descr = WebUtils.FindAllApplicableHtmlNodes(pathy, (node) =>
					{
						if (node.Name != "tr")
						{
							return false;
						}

						int numTD = 0;
						var found = false;

						foreach (var child in node.ChildNodes)
						{
							if (child.Name != "td")
							{
								continue;
							}

							++numTD;

							foreach (var depth in child.ChildNodes)
							{
								if (depth.Name == "strong")
								{
									if (String.CompareOrdinal(depth.InnerText.Trim(), "Card # / Rarity") == 0)
									{
										found = true;
									}
								}
							}
						}

						return numTD == 2 && found;
					}).FirstOrDefault();

					var setnd = WebUtils.FindAllApplicableHtmlNodes(pathy, (node) =>
					{
						if (node.Name != "tr")
						{
							return false;
						}

						int numTD = 0;
						var found = false;

						foreach (var child in node.ChildNodes)
						{
							if (child.Name != "td")
							{
								continue;
							}

							++numTD;

							foreach (var depth in child.ChildNodes)
							{
								if (depth.Name == "strong")
								{
									if (String.CompareOrdinal(depth.InnerText.Trim(), "Set") == 0)
									{
										found = true;
									}
								}
							}
						}

						return numTD == 2 && found;
					}).FirstOrDefault();

					if (descr is not null)
					{
						var value = WebUtils.GetHtmlNodeContentByPath(descr, "td[2]");
						var split = value.Split('/', StringSplitOptions.RemoveEmptyEntries);

						if (split.Length >= 1)
						{
							this.m_setCode = split[0].Trim();

							if (this.m_name.Contains(this.m_setCode))
							{
								this.m_name = this.m_name.Replace(this.m_setCode, String.Empty).Trim();
							}
						}
						
						if (split.Length >= 2)
						{
							this.m_rarity = split[1].Trim();
						}
					}

					if (setnd is not null)
					{
						this.m_setName = WebUtils.GetHtmlNodeContentByPath(setnd, "td[2]").Trim();
					}
				}
			}

			if (price is not null)
			{
				var split = price.InnerText.Split(new char[] { ' ', '$' }, StringSplitOptions.RemoveEmptyEntries);

				if (split.Length > 0)
				{
					_ = Double.TryParse(split[0], out this.m_price);
				}
			}
			else
			{
				price = WebUtils.FindHtmlNodeWithAttributeRecursive(document.DocumentNode, "span", "class", "text-success");

				if (price is not null)
				{
					var split = price.InnerText.Split(new char[] { ' ', '$' }, StringSplitOptions.RemoveEmptyEntries);

					if (split.Length > 0)
					{
						_ = Double.TryParse(split[0], out this.m_price);
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}
