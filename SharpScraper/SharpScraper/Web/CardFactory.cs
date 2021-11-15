using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
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

		private static bool TryGetDomainFromURL(string url, [NotNullWhen(true)] out string? domain)
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
			this.RegisterTactic<TCGMPTactic>(TCGMPTactic.Domain);
			this.RegisterTactic<CardRushTactic>(CardRushTactic.Domain);
			this.RegisterTactic<TrollAndToadTactic>(TrollAndToadTactic.Domain);
		}

		public void RegisterTactic<T>(string? domain) where T : ICardTactic, new()
		{
			this.m_domainToTactic[domain ?? String.Empty] = () => new T();
		}

		public async Task ExportAsync(string path, ExportType exportType)
		{
			// #TODO
			_ = path;
			_ = exportType;
			await Task.Delay(0);
		}

		public async Task ParseAsync(string url)
		{
			this.m_cardTactics.Add(NullTactic.Null);

			if (!CardFactory.TryGetDomainFromURL(url, out var domain))
			{
				return;
			}

			if (!this.m_domainToTactic.TryGetValue(domain, out var activator))
			{
				return;
			}

			var document = await WebUtils.TryReceiveHtmlPageAsync(url);

			if (document is null)
			{
				return;
			}

			var cardTactic = activator.Invoke();

			await cardTactic.Parse(document);

			this.m_cardTactics[^1] = cardTactic;
		}
	}
}
