using System;

namespace SharpScraper
{
	public static class Extensions
	{
		public static string RawConcat(this string[]? array, int start, int count, string separator)
		{
			if (array is null || start < 0 || start + count > array.Length)
			{
				return String.Empty;
			}

			string result = array[start];

			for (int i = start + 1; i < start + count; ++i)
			{
				result += separator + array[i];
			}

			return result;
		}
	}
}
