using Microsoft.EntityFrameworkCore;
using Rhym.Api.Data;

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

		var result = (await _context.Words.ToListAsync()).Where(word =>
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
			while (foundEnumerator.Current != null) {
				if (wordEnumerator.Current == null)
				{
					return false;
				}
				string foundSyllable = RemoveStress(foundEnumerator);
				string wordSyllable = RemoveStress(wordEnumerator);
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
		}).Select(word => word.WordKey).ToList();

		return result;
	}

	private static string RemoveStress(IEnumerator<string> enumerator)
	{
		return enumerator.Current.Length > 2 ? enumerator.Current.Substring(0, 2) : enumerator.Current;
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
