using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SharpScraper
{
	class Program
	{
		static void Main(string[] args)
		{
			var task = Program.Run(args);

			while (!task.IsCompleted)
			{
				// #TODO: loading thingy
			}

			Console.WriteLine("Done!");
		}

		private static async Task Run(string[] args)
		{
			args = new string[]
			{
				"https://www.cardrush-pokemon.jp/product/3100",
				"https://www.cardrush-pokemon.jp/product/3850",
				"https://www.cardrush-pokemon.jp/product/3855",
				"https://www.cardrush-pokemon.jp/product/3856",
				"https://tcgmp.jp/product/detail?id=203727&referer=1",
			}; // for now

			if (args.Length == 0)
			{
				Console.WriteLine($"Number of arguments should be at least 1 (URL)");
			}

			var factory = new Web.CardFactory();

			factory.Init();

			var tasks = new Task[args.Length];

			for (int i = 0; i < args.Length; ++i)
			{
				tasks[i] = factory.Parse(args[i]);
			}

			await Task.WhenAll(tasks);

			// #TODO export?

			Debugger.Break();
		}
	}
}
