using Rhym.Api.Data;
using Rhym.Api.Models;
using System.Data.Common;
using System.IO;

namespace Rhym.Api;

public class Seeder
{
	public static async Task Seed(AppDbContext db)
	{
		if (!db.Words.Any())
		{
			string? line;
			try
			{
				string? projectDirectory = Environment.CurrentDirectory;
				if (projectDirectory == null)
				{
					throw new InvalidOperationException("Could not find directory");
				}
				projectDirectory = Path.Combine(projectDirectory, "Dictionary.txt");
				StreamReader reader = new StreamReader(projectDirectory);
				line = reader.ReadLine();

				while (line != null)
				{
					if (!line.StartsWith("#"))
					{
						string[] array = line.Split("  ");
						if (array.Length != 2)
						{
							throw new InvalidOperationException("Text file is not properly formatted");
						}

						var syllables = array[1].Split(" - ");
						var pronunciation = String.Join(' ', syllables).Split(' ');

						Word word = new Word
						{
							WordKey = array[0].Trim(),
							Phonemes = pronunciation,
							Syllables = syllables
						};
						await db.Words.AddAsync(word);
					}
					line = reader.ReadLine();
				}
				reader.Close();
				await db.SaveChangesAsync();
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("FileNotFoundException: " + e.Message);
			}
		}
	}
}
