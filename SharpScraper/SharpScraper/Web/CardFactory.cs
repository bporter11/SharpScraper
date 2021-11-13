using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class CardFactory
	{
		public enum ExportType
		{
			XLSX,
			CSV, // ???
		}

		private readonly Dictionary<string, Func<ICardTactic>> m_domainToTactic;
		private readonly List<ICardTactic> m_cardTactics;

		public ReadOnlyCollection<ICardTactic> LoadedCards => new(this.m_cardTactics);

		public CardFactory()
		{
			this.m_domainToTactic = new Dictionary<string, Func<ICardTactic>>();
			this.m_cardTactics = new List<ICardTactic>();
		}

		private static bool TryGetDomainFromURL(string url, out string domain)
		{
			try
			{
				domain = new Uri(url).Host;
				return true;
			}
			catch
			{
				domain = null;
				return false;
			}
		}

		public void Init()
		{
			this.m_domainToTactic.Add(CardCrushTactic.Domain, () => new CardCrushTactic());
			this.m_domainToTactic.Add(TCGMPTactic.Domain, () => new TCGMPTactic());
			// #TODO more in case we need
		}

		public async Task Export(string path, ExportType exportType)
		{
			// #TODO
			await Task.Delay(0);
		}

		public async Task Parse(string url)
		{
			if (!CardFactory.TryGetDomainFromURL(url, out var domain))
			{
				throw new InvalidHyperlinkException($"Unable to get domain from {url} hyperlink");
			}

			if (!this.m_domainToTactic.TryGetValue(domain, out var activator))
			{
				throw new CardTacticNotRegisteredException($"Unable to find parser for {domain} domain");
			}

			var document = await WebUtils.TryReceiveHtmlPage(url);

			var cardTactic = activator.Invoke();

			await cardTactic.Parse(document);

			this.m_cardTactics.Add(cardTactic);
		}
	}
}
