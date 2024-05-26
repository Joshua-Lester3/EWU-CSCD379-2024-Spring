using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Rhym.Api.Data;
using Rhym.Api.Models;

namespace Rhym.Api.Services;

public class WordService
{
	private readonly AppDbContext _context;

	public WordService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<string>> GetPerfectRhymes(string givenWord)
	{
		var foundWord = await _context.Words.FirstOrDefaultAsync(word => word.WordKey.Equals(givenWord.ToUpper()));

		if (foundWord == null)
		{
			throw new InvalidOperationException("Word not in dictionary");
		}
		var foundWordPhonemes = foundWord.Phonemes.Reverse();

		var result = (await _context.Words.ToListAsync())
			.Where(word => FilterPerfectRhymes(word, givenWord, foundWordPhonemes))
			.Select(word => word.WordKey)
			.ToList();

		return result;
	}

	public bool FilterPerfectRhymes(Word word, string givenWord, IEnumerable<string> foundWordPhonemes)
	{
		if (word.WordKey.Equals(givenWord.ToUpper()))
		{
			return false;
		}
		var pronunciation = word.Phonemes.Reverse();
		var foundEnumerator = foundWordPhonemes.GetEnumerator();
		var wordEnumerator = pronunciation.GetEnumerator();
		foundEnumerator.MoveNext();
		wordEnumerator.MoveNext();
		while (foundEnumerator.Current != null)
		{
			if (wordEnumerator.Current == null)
			{
				return false;
			}
			string foundSyllable = RemoveStress(foundEnumerator.Current);
			string wordSyllable = RemoveStress(wordEnumerator.Current);
			if (!foundSyllable.Equals(wordSyllable))
			{
				foundEnumerator.Dispose();
				wordEnumerator.Dispose();
				return false;
			}
			foundEnumerator.MoveNext();
			wordEnumerator.MoveNext();
		}
		wordEnumerator.Dispose();
		foundEnumerator.Dispose();

		return true;
	}

	private static string RemoveStress(string syllable)
	{
		return syllable.Length > 2 ? syllable.Substring(0, 2) : syllable;
	}

	public async Task<string[]> GetPhonemes(string givenWord)
	{
		var foundWord = await _context.Words.FirstOrDefaultAsync(word => word.WordKey.Equals(givenWord.ToUpper()));
		return foundWord?.Phonemes ?? [givenWord]; // Return given word if not found
	}

	public async Task<string[]> GetSyllables(string givenWord)
	{
		var foundWord = await _context.Words.FirstOrDefaultAsync(word => word.WordKey.Equals(givenWord.ToUpper()));
		return foundWord?.Syllables ?? [givenWord]; // Return given word if not found
	}

	public async Task<List<string>> GetImperfectRhymes(string phonemesString)
	{
		string[] phonemes = phonemesString.Split(' ');
		var result = (await _context.Words.ToListAsync())
			.Where(word =>
			{
				int index = 0;
				foreach (string dbSyllable in word.Phonemes)
				{
					if (index < phonemes.Length)
					{
						var dbSyllableStressless = RemoveStress(dbSyllable);
						var syllableStressless = RemoveStress(phonemes[index]);
						if (dbSyllableStressless.Equals(syllableStressless))
						{
							index++;
						}
					}
				}
				return index == phonemes.Length;
			})
			.Select(word => word.WordKey)
			.ToList();

		return result;
	}

	public List<string> Vowels = new List<string>
	{
		"AA", "AE", "AH", "AO", "AW", "AY",
		"EH", "ER", "EY", "IH", "IY", "OW",
		"OY", "UH", "UW",
	};

	public List<string> Consonants = new List<string>
	{
		"B", "CH", "D", "DH", "F", "G", "HH",
		"JH", "K", "L", "M", "N", "NG", "P",
		"R", "S", "SH", "T", "TH", "V", "W",
		"Y", "Z", "ZH"
	};
}
