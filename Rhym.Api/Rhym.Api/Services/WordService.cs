using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rhym.Api.Data;
using Rhym.Api.Dtos;
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
		return foundWord?.SyllablesPronunciation ?? [givenWord]; // Return given word if not found
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

	public async Task<List<string>> GetPronunciationToPlain(string word)
	{
		word = word.Trim().ToUpper();
		var foundSyllables = await _context.Syllables.FirstOrDefaultAsync(syllables => syllables.WordKey.Equals(word));
		if (foundSyllables == null)
		{
			return [];
		}
		return foundSyllables.PlainTextSyllables.ToList();
	}

	public async Task<bool> AddWord(WordDto dto)
	{
		if (dto.Word.IsNullOrEmpty() || dto.SyllablesPronunciation.Length == 0 || dto.Phonemes.Length == 0 || dto.PlainTextSyllables.Length == 0)
		{
			throw new ArgumentException("Invalid arguments. All arrays must have at least one element and Word must not be null or empty.");
		}
		var foundWord = await _context.Words.FirstOrDefaultAsync(word => word.WordKey.Equals(dto.Word.ToLower()));
		if (foundWord is not null)
		{
			return false;
		}

		Word word = new Word
		{
			WordKey = dto.Word,
			Phonemes = dto.Phonemes,
			SyllablesPronunciation = dto.SyllablesPronunciation,
		};
		await _context.Words.AddAsync(word);
		await _context.SaveChangesAsync();


		Syllable syllable = new Syllable
		{
			WordKey = dto.Word,
			PlainTextSyllables = dto.PlainTextSyllables,
			WordId = word.WordId,
		};

		await _context.Syllables.AddAsync(syllable);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<PaginatedWordsDto> GetWordListPaginated(int countPerPage, int pageNumber)
	{
		var words = await _context.Words
			.OrderBy(word => word.WordKey.ToLower())
			.Skip(pageNumber * countPerPage)
			.Take(countPerPage)
			.Include(word => word.Syllable)
			.Select(word => new WordDto
			{
				Word = word.WordKey,
				Phonemes = word.Phonemes,
				SyllablesPronunciation = word.SyllablesPronunciation,
				PlainTextSyllables = word.Syllable!.PlainTextSyllables, // Syllable should never be null here
			})
			.ToListAsync();
		PaginatedWordsDto result = new PaginatedWordsDto
		{
			Words = words,
			Pages = await _context.Words.CountAsync()
		};
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