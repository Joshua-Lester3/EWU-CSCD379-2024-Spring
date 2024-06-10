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

		var result = (await _context.Rhymes.ToListAsync())
			.Where(rhyme => FilterPerfectRhymes(rhyme, givenWord, foundWordPhonemes))
			.Select(rhyme => rhyme.Word)
			.ToList();

		return result;
	}

	public bool FilterPerfectRhymes(Rhyme rhyme, string givenWord, IEnumerable<string> foundWordPhonemes)
	{
		if (rhyme.Word.Equals(givenWord.ToUpper()))
		{
			return false;
		}
		var pronunciation = rhyme.Phonemes.Reverse();
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
		var foundWord = await _context.Rhymes.FirstOrDefaultAsync(word => word.Word.Equals(givenWord.ToUpper()));
		return foundWord?.Phonemes ?? [givenWord]; // Return given word if not found
	}

	public async Task<string[]> GetSyllables(string givenWord)
	{
		var foundWord = await _context.Rhymes.FirstOrDefaultAsync(word => word.Word.Equals(givenWord.ToUpper()));
		return foundWord?.SyllablesPronunciation ?? [givenWord]; // Return given word if not found
	}

	public async Task<List<string>> GetImperfectRhymes(string phonemesString)
	{
		string[] phonemes = phonemesString.Split(' ');
		var result = (await _context.Rhymes.ToListAsync())
			.Where(rhyme =>
			{
				int index = 0;
				foreach (string dbSyllable in rhyme.Phonemes)
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
			.Select(rhyme => rhyme.Word)
			.ToList();

		return result;
	}

	public async Task<List<string>> GetPronunciationToPlain(string word)
	{
		word = word.Trim().ToUpper();
		var foundSyllables = await _context.Rhymes.FirstOrDefaultAsync(rhyme => rhyme.Word.Equals(word));
		if (foundSyllables == null)
		{
			return [];
		}
		return foundSyllables.PlainTextSyllables.ToList();
	}

	public async Task<bool> AddWord(WordDto dto)
	{
		
		var foundWord = await _context.Rhymes.FirstOrDefaultAsync(rhyme => rhyme.Word.Equals(dto.Word.ToLower()));
		if (foundWord is not null)
		{
			return false;
		}

		var phonemes = new List<string>();
		for (int index = 0; index < dto.SyllablesPronunciation.Length; index++)
		{
			dto.SyllablesPronunciation[index] = dto.SyllablesPronunciation[index].ToUpper();
			dto.PlainTextSyllables[index] = dto.PlainTextSyllables[index].ToUpper();
		}
		foreach (string pronunciationSyllable in dto.SyllablesPronunciation)
		{
			var splitSyllable = pronunciationSyllable.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			foreach (string phoneme in splitSyllable)
			{
				phonemes.Add(phoneme);
			}
		}

		Rhyme rhyme = new Rhyme
		{
			Word = dto.Word.ToUpper(),
			Phonemes = phonemes.ToArray(),
			SyllablesPronunciation = dto.SyllablesPronunciation,
			PlainTextSyllables = dto.PlainTextSyllables
		};

		await _context.Rhymes.AddAsync(rhyme);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<PaginatedWordsDto> GetWordListPaginated(int countPerPage, int pageNumber)
	{
		var words = _context.Rhymes
			.OrderBy(rhyme => rhyme.Word.ToLower())
			.Skip(pageNumber * countPerPage)
			.Take(countPerPage)
			.Select(rhyme => new WordDto
			{
				Word = rhyme.Word,
				SyllablesPronunciation = rhyme.SyllablesPronunciation,
				PlainTextSyllables = rhyme.PlainTextSyllables,
			}).ToList();
		int numberOfRhymes = await _context.Rhymes.CountAsync();
		int rhymesDivided = numberOfRhymes / countPerPage;
		if (numberOfRhymes % countPerPage != 0) {
			rhymesDivided++;
		}
		PaginatedWordsDto result = new PaginatedWordsDto
		{
			Words = words,
			Pages = rhymesDivided,
		};
		return result;
	}

	//public async Task<List<WordDto>> GetWordsInDictionaryNotAdded()
	//{
	//	var currentDictionary = await _context.Rhymes.ToListAsync();

	//	await _context.Words.Where(word => currentDictionary.Contains(word.WordKey.ToUpper()));
	//}

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