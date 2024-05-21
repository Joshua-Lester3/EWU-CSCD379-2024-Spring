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
		var foundWordPronunciation = foundWord.Pronunciation.Split(' ').Reverse();

		var result = (await _context.Words.ToListAsync())
			.Where(word => FilterPerfectRhymes(word, givenWord, foundWordPronunciation))
			.Select(word => word.WordKey)
			.ToList();

		return result;
	}

	public bool FilterPerfectRhymes(Word word, string givenWord, IEnumerable<string> foundWordPronunciation)
	{
		if (word.WordKey.Equals(givenWord.ToUpper()))
		{
			return false;
		}
		var pronunciation = word.Pronunciation.Split(' ').Reverse();
		var foundEnumerator = foundWordPronunciation.GetEnumerator();
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

	public async Task<string?> GetPronunciation(string givenWord)
	{
		var foundWord = await _context.Words.FirstOrDefaultAsync(word => word.WordKey.Equals(givenWord.ToUpper()));
		return foundWord?.Pronunciation;
	}

	public async Task<List<string>> GetImperfectRhymes(string[] syllables)
	{
		var result = (await _context.Words.ToListAsync())
			.Where(word =>
			{
				var dbSyllables = word.Pronunciation.Split(' ');
				int index = 0;
				foreach (string dbSyllable in dbSyllables)
				{
					var dbSyllableStressless = RemoveStress(dbSyllable);
					var syllableStressless = RemoveStress(syllables[index]);
					if (dbSyllableStressless.Equals(syllableStressless)) {
						index++;
					}
				}
				return index == syllables.Length;
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
