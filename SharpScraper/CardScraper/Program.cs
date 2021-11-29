using SharpScraper.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CardScraper
{
	class Program
	{
		private static async Task Main(string[] args)
		{
			// args[0] = MODE
			// args[1] = EXPORT
			// args[2] = PATH
			// args[.] = URLs

			if (args.Length < 4)
			{
				Console.WriteLine("Invalid number of passed arguments. Usage: [MODE] [EXPORT_TYPE] [EXPORT_PATH] [MODE_ARGS]");
				return;
			}

			if (String.Compare("file", args[0], StringComparison.OrdinalIgnoreCase) == 0)
			{
				await Program.ProcessFromFile(args);
			}
			else if (String.Compare("args", args[0], StringComparison.OrdinalIgnoreCase) == 0)
			{
				await Program.ProcessFromArgs(args);
			}
			else
			{
				Console.WriteLine("Invalid MODE passed. Valid types are: [file], [args]");
				return;
			}

			Console.WriteLine("Done!");
		}

		private static async Task ProcessFromFile(string[] args)
		{
			Console.WriteLine("Processing files...");

			var allURLs = new List<string>();

			for (int i = 3; i < args.Length; ++i)
			{
				if (!File.Exists(args[i]))
				{
					Console.WriteLine($"Warning: file [{args[i]}] does not exist!");
					continue;
				}

				allURLs.AddRange(File.ReadAllLines(args[i]));
			}

			var factory = new CardFactory();

			factory.Init();

			Console.WriteLine("Processing URLs...");

			var tasks = new Task[allURLs.Count];

			for (int i = 0; i < tasks.Length; ++i)
			{
				Console.WriteLine($"Processing URL [{allURLs[i]}]");
				tasks[i] = factory.ParseAsync(allURLs[i]);
			}

			await Task.WhenAll(tasks);

			for (int i = 0; i < allURLs.Count; ++i)
			{
				if (!factory.LoadedCards.TryGetValue(allURLs[i], out var card) || card.IsNull)
				{
					Console.WriteLine($"Warning: unable to parse card website with URL [{allURLs[i]}]");
				}
			}

			Console.WriteLine("Exporting database...");

			int result = await factory.ExportAsync(args[2], args[1]);

			if (result < 0)
			{
				Console.WriteLine($"Failed to export data with [{args[1]}] type");
			}
			else
			{
				Console.WriteLine($"Exported {result} URLs to path [{args[2]}]");
			}
		}

		private static async Task ProcessFromArgs(string[] args)
		{
			Console.WriteLine("Processing arguments...");

			for (int i = 3; i < args.Length; ++i)
			{
				if (String.IsNullOrWhiteSpace(args[i]))
				{
					Console.WriteLine($"Warning: argument [{args[i]}] is null, empty or whitespace!");
				}
			}

			var factory = new CardFactory();

			factory.Init();

			var tasks = new Task[args.Length - 3];

			for (int i = 0; i < tasks.Length; ++i)
			{
				tasks[i] = factory.ParseAsync(args[i + 3]);
			}

			await Task.WhenAll(tasks);

			for (int i = 3; i < args.Length; ++i)
			{
				if (!factory.LoadedCards.TryGetValue(args[i], out var card) || card.IsNull)
				{
					Console.WriteLine($"Warning: unable to parse card website with URL [{args[i]}]");
				}
			}

			Console.WriteLine("Exporting database...");

			int result = await factory.ExportAsync(args[2], args[1]);

			if (result < 0)
			{
				Console.WriteLine($"Failed to export data with [{args[1]}] type");
			}
			else
			{
				Console.WriteLine($"Exported {result} URLs to path [{args[2]}]");
			}
		}
	}
}
