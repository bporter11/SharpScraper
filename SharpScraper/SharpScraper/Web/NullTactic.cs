using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace SharpScraper.Web
{
	/// <summary>
	/// Represents default nullable implementation of <see cref="ICardTactic"/>.
	/// </summary>
	public class NullTactic : ICardTactic
	{
		/// <summary>
		/// Represents empty <see cref="NullTactic"/>. This field is read-only.
		/// </summary>
		public static readonly NullTactic Null = new();

		/// <summary>
		/// Returns constant <see langword="true"/>.
		/// </summary>
		public bool IsNull => true;

		/// <summary>
		/// This property is not implemented and throws <see cref="NotImplementedException"/>.
		/// </summary>
		public string Name => throw new NotImplementedException();

		/// <summary>
		/// This property is not implemented and throws <see cref="NotImplementedException"/>.
		/// </summary>
		public double Price => throw new NotImplementedException();

		/// <summary>
		/// This property is not implemented and throws <see cref="NotImplementedException"/>.
		/// </summary>
		public string Rarity => throw new NotImplementedException();

		/// <summary>
		/// This property is not implemented and throws <see cref="NotImplementedException"/>.
		/// </summary>
		public string SetCode => throw new NotImplementedException();

		/// <summary>
		/// This property is not implemented and throws <see cref="NotImplementedException"/>.
		/// </summary>
		public string SetName => throw new NotImplementedException();

		/// <summary>
		/// This method is not implemented and throws <see cref="NotImplementedException"/>.
		/// </summary>
		public Task Parse(HtmlDocument document) => throw new NotImplementedException();
	}
}
