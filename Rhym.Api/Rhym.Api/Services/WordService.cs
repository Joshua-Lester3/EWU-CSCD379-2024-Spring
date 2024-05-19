using Rhym.Api.Data;

namespace Rhym.Api.Services;

public class WordService
{
	private readonly AppDbContext _context;

	public WordService(AppDbContext context)
	{
		_context = context;
	}

	public List<string> GetPerfectRhymes(string givenWord)
	{
		var wordPronunciation = _context.Words.FirstOrDefault(word => word.WordKey.Equals(givenWord.ToUpper()));

		if (wordPronunciation == null)
		{
			throw new InvalidOperationException("Word not in dictionary");
		}



		return [wordPronunciation.WordKey];
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
