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
	/// <summary>
	/// Provides factory pattern for parsing and exporting databases of cards.
	/// </summary>
	public class CardFactory
	{
		private readonly Dictionary<string, Func<ICardTactic>> m_domainToTactic;
		private readonly Dictionary<string, Func<IExportBase>> m_stringToExport;
		private readonly ReadOnlyDictionary<string, ICardTactic> m_readonlyCards;
		private readonly ConcurrentDictionary<string, ICardTactic> m_cardTactics;

		/// <summary>
		/// Represents a read-only dictionary of parsed URLs and <see cref="ICardTactic"/>.
		/// </summary>
		public ReadOnlyDictionary<string, ICardTactic> LoadedCards => this.m_readonlyCards;

		/// <summary>
		/// Constructs new instance of <see cref="CardFactory"/>.
		/// </summary>
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

		/// <summary>
		/// Initializes current instance of <see cref="CardFactory"/> with default supported <see cref="ICardTactic"/> and <see cref="IExportBase"/> classes.
		/// </summary>
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

		/// <summary>
		/// Checks whether <see cref="ICardTactic"/> with domain provided is registered in the current factory.
		/// </summary>
		/// <param name="domain">Domain of <see cref="ICardTactic"/> as a <see cref="String"/>.</param>
		/// <returns><see langword="true"/> if <see cref="ICardTactic"/> with domain provided is registered; otherwise, <see langword="false"/>.</returns>
		public bool IsTacticRegistered(string? domain)
		{
			return this.m_domainToTactic.ContainsKey(domain ?? String.Empty);
		}

		/// <summary>
		/// Checks whether <see cref="IExportBase"/> with identifier provided is registered in the current factory.
		/// </summary>
		/// <param name="export">Export identifier of <see cref="IExportBase"/> as a <see cref="String"/>.</param>
		/// <returns><see langword="true"/> if <see cref="IExportBase"/> with identifier provided is registered; otherwise, <see langword="false"/>.</returns>
		public bool IsExportRegistered(string? export)
		{
			return this.m_stringToExport.ContainsKey(export ?? String.Empty);
		}

		/// <summary>
		/// Registers <see cref="ICardTactic"/> type provided under the domain specified in the current factory.
		/// </summary>
		/// <typeparam name="T"><see cref="ICardTactic"/> type to register.</typeparam>
		/// <param name="domain">Domain of <see cref="ICardTactic"/> as a <see cref="String"/>.</param>
		public void RegisterTactic<T>(string? domain) where T : ICardTactic, new()
		{
			this.m_domainToTactic[domain ?? String.Empty] = () => new T();
		}

		/// <summary>
		/// Registers <see cref="IExportBase"/> type provided under the identifier specified in the current factory.
		/// </summary>
		/// <typeparam name="T"><see cref="IExportBase"/> type to register.</typeparam>
		/// <param name="export">Identifier of <see cref="IExportBase"/> as a <see cref="String"/>.</param>
		public void RegisterExport<T>(string? export) where T : IExportBase, new()
		{
			this.m_stringToExport[export ?? String.Empty] = () => new T();
		}

		/// <summary>
		/// Asynchronously exports all parsed <see cref="ICardTactic"/> into the stream provided with export type specified.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> to export data into.</param>
		/// <param name="export">Identifier of <see cref="IExportBase"/> to use.</param>
		/// <returns>Number of exported cards as <see cref="Int32"/>.</returns>
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

		/// <summary>
		/// Asynchronously exports all parsed <see cref="ICardTactic"/> into file path provided with export type specified.
		/// </summary>
		/// <param name="iopath">File path to export data into.</param>
		/// <param name="export">Identifier of <see cref="IExportBase"/> to use.</param>
		/// <returns>Number of exported cards as <see cref="Int32"/>.</returns>
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

		/// <summary>
		/// Asynchronously parses card data with URL provided and using according registered <see cref="ICardTactic"/>.
		/// </summary>
		/// <param name="url">URL of card to parse and store in the current factory.</param>
		/// <returns><see cref="Task.CompletedTask"/> on successful parsing.</returns>
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
