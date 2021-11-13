using System;

namespace SharpScraper.Web
{
	public class InvalidHyperlinkException : Exception
	{
		public InvalidHyperlinkException() : base("Specified domain link is invalid")
		{
		}

		public InvalidHyperlinkException(string? message) : base(message)
		{
		}

		public InvalidHyperlinkException(string? message, Exception? inner) : base(message, inner)
		{
		}
	}
}
