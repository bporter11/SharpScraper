using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	public class CardFactory
	{
		private readonly Dictionary<string, Func<ICardTactic>> m_domainToTactic;
		private readonly Dictionary<string, Func<IExportBase>> m_stringToExport;
		private readonly ReadOnlyDictionary<string, ICardTactic> m_readonlyCards;
		private readonly ConcurrentDictionary<string, ICardTactic> m_cardTactics;

		public ReadOnlyDictionary<string, ICardTactic> LoadedCards => this.m_readonlyCards;

		public CardFactory()
		{
			this.m_domainToTactic = new Dictionary<string, Func<ICardTactic>>();
			this.m_stringToExport = new Dictionary<string, Func<IExportBase>>();
			this.m_cardTactics = new ConcurrentDictionary<string, ICardTactic>();
			this.m_readonlyCards = new ReadOnlyDictionary<string, ICardTactic>(this.m_cardTactics);
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
			this.RegisterTactic<CardMarketTactic>(CardMarketTactic.Domain);
			this.RegisterTactic<PokemonWizardTactic>(PokemonWizardTactic.Domain);

			this.RegisterExport<XMLExportBase>(XMLExportBase.Name);
			this.RegisterExport<CSVExportBase>(CSVExportBase.Name);
			this.RegisterExport<TextExportBase>(TextExportBase.Name);
			this.RegisterExport<XLSXExportBase>(XLSXExportBase.Name);
		}

		public bool IsTacticRegistered(string? domain)
		{
			return this.m_domainToTactic.ContainsKey(domain ?? String.Empty);
		}
		public bool IsExportRegistered(string? export)
		{
			return this.m_stringToExport.ContainsKey(export ?? String.Empty);
		}

		public void RegisterTactic<T>(string? domain) where T : ICardTactic, new()
		{
			this.m_domainToTactic[domain ?? String.Empty] = () => new T();
		}
		public void RegisterExport<T>(string? export) where T : IExportBase, new()
		{
			this.m_stringToExport[export ?? String.Empty] = () => new T();
		}

		public async Task<int> ExportAsync(Stream? stream, string export)
		{
			if (stream is null)
			{
				return -1;
			}

			if (!this.m_stringToExport.TryGetValue(export, out var activator))
			{
				return -1;
			}

			var details = this.m_cardTactics.Where(_ => !_.Value.IsNull).ToDictionary(_ => _.Key, _ => _.Value);

			await activator.Invoke().Export(stream, details);

			return details.Count;
		}
		public async Task<int> ExportAsync(string? iopath, string export)
		{
			if (String.IsNullOrEmpty(iopath))
			{
				return -1;
			}

			using (var stream = File.Open(iopath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				return await this.ExportAsync(stream, export);
			}
		}

		public async Task ParseAsync(string url)
		{
			this.m_cardTactics[url] = NullTactic.Null;

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

			this.m_cardTactics[url] = cardTactic;
		}
	}
}
