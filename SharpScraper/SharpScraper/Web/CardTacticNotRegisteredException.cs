using System;

namespace SharpScraper.Web
{
	public class CardTacticNotRegisteredException : Exception
	{
		public CardTacticNotRegisteredException() : base("Unable to find Card Website Parser for the specified domain")
		{
		}

		public CardTacticNotRegisteredException(string? message) : base(message)
		{
		}

		public CardTacticNotRegisteredException(string? message, Exception? inner) : base(message, inner)
		{
		}
	}
}
