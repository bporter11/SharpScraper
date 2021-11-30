using HtmlAgilityPack;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Provides basic functionality for parsing and storing website-dependent card information.
	/// </summary>
	public interface ICardTactic
	{
		/// <summary>
		/// Returns <see langword="true"/> if this class is a default nullable object implemention; otherwise, <see langword="false"/>.
		/// </summary>
		public bool IsNull { get; }

		/// <summary>
		/// Name of the card parsed; if empty, returns <see cref="System.String.Empty"/>.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Price of the card parsed; if empty, returns default <see cref="System.Double"/>.
		/// </summary>
		public double Price { get; }

		/// <summary>
		/// Rarity of the card parsed; if empty, returns <see cref="System.String.Empty"/>.
		/// </summary>
		public string Rarity { get; }

		/// <summary>
		/// Set Code of the card parsed; if empty, returns <see cref="System.String.Empty"/>.
		/// </summary>
		public string SetCode { get; }

		/// <summary>
		/// Set Name of the card parsed; if empty, returns <see cref="System.String.Empty"/>.
		/// </summary>
		public string SetName { get; }

		/// <summary>
		/// Parses card information based on the <see cref="HtmlDocument"/> provided.
		/// </summary>
		/// <param name="document"><see cref="HtmlDocument"/> to get card information from.</param>
		/// <returns><see cref="Task.CompletedTask"/> on successful completion.</returns>
		public Task Parse(HtmlDocument document);
	}
}
