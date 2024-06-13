using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
						var wordKey = array[0].Trim().ToLower();

						Word word = new Word
						{
							WordKey = wordKey,
							Phonemes = pronunciation,
							SyllablesPronunciation = syllables,
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
		if (!db.Syllables.Any())
		{
			string? line;
			try
			{
				string? projectDirectory = Environment.CurrentDirectory;
				if (projectDirectory == null)
				{
					throw new InvalidOperationException("Could not find directory");
				}
				projectDirectory = Path.Combine(projectDirectory, "Syllables.txt");
				StreamReader reader = new StreamReader(projectDirectory);
				line = reader.ReadLine();
				while (!line.IsNullOrEmpty())
				{
					string[] syllables = line!.Split(';');

					string word = String.Join("", syllables).ToUpper();
					Word? foundWord = await db.Words.FirstOrDefaultAsync(dbWord => dbWord.WordKey.Equals(word));
					if (foundWord != null)
					{
						Syllable syllable = new Syllable
						{
							WordKey = word,
							PlainTextSyllables = syllables,
							WordId = foundWord.WordId,
						};
						await db.Syllables.AddAsync(syllable);
					}
					line = reader.ReadLine();
				}
				await db.SaveChangesAsync();
				reader.Close();
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("FileNotFoundException: " + e.Message);
			}
		}

		if (!db.Rhymes.Any())
		{
			Rhyme rhyme2 = new Rhyme
			{
				Word = "Hi",
				Phonemes = [],
				SyllablesPronunciation = [],
				PlainTextSyllables = [],
			};
			await db.Rhymes.AddAsync(rhyme2);
			await db.SaveChangesAsync();

			var syllables = await db.Syllables.Include(syllable => syllable.Word).ToListAsync();
			foreach (Syllable syllable in syllables)
			{
				if (syllable.Word != null)
				{
					Rhyme rhyme = new Rhyme
					{
						Word = syllable.WordKey,
						Phonemes = syllable.Word.Phonemes,
						SyllablesPronunciation = syllable.Word.SyllablesPronunciation,
						PlainTextSyllables = syllable.PlainTextSyllables,
					};
					await db.Rhymes.AddAsync(rhyme);
				}
			}
			await db.SaveChangesAsync();
		}
	}
}
