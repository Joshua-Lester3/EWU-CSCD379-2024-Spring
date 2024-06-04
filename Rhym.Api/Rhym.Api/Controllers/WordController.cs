using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rhym.Api.Dtos;
using Rhym.Api.Services;

namespace Rhym.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WordController
{
	private readonly WordService _service;

	public WordController(WordService service)
	{
		_service = service;
	}

	[HttpGet("PerfectRhyme")]
	public async Task<List<string>> GetPerfectRhymes(string word)
	{
		return await _service.GetPerfectRhymes(word);
	}

	[HttpGet("Pronunciation")]
	public async Task<string[]?> GetPronunciation(string word)
	{
		return await _service.GetPhonemes(word);
	}

	[HttpPost("PoemPronunciationPretty")]
	public async Task<string> GetPoemPronunciationPretty(DocumentDataDto poem)
	{
		var lines = poem.Content.Split('\n');
		var wordsByLine = new List<List<string>>();
		foreach (string line in lines)
		{
			var wordsInLine = line.Split(' ').ToList();
			wordsByLine.Add(wordsInLine);
		}
		var pronunciations = new List<List<string[]>>();
		foreach (List<string> lineOfWords in wordsByLine)
		{
			List<string[]> addedLineOfPronunciations = new();
			pronunciations.Add(addedLineOfPronunciations);
			foreach (string word in lineOfWords)
			{
				var pronunciation = await _service.GetSyllables(word);
				addedLineOfPronunciations.Add(pronunciation);
			}
		}

		string poemPronunciation = "";
		foreach (List<string[]> lineOfPronunciations in pronunciations)
		{
			foreach (string[] pronunciation in lineOfPronunciations)
			{
				poemPronunciation += "[ ";
				poemPronunciation += String.Join(" - ", pronunciation);
				poemPronunciation += " ]  ";
			}
			poemPronunciation = poemPronunciation.Trim();
			poemPronunciation += "\n";
		}
		return poemPronunciation;
	}

	[HttpPost("PoemPronunciation")]
	public async Task<PoemPair> GetPoemPronunciation(DocumentDataDto poem)
	{
		var lines = poem.Content.Split('\n');
		var wordsByLine = new List<List<string>>();
		foreach (string line in lines)
		{
			var wordsInLine = line.Split(' ').ToList();
			wordsByLine.Add(wordsInLine);
		}
		var pronunciations = new List<List<string[]>>();
		var words = "";
		foreach (List<string> lineOfWords in wordsByLine)
		{
			List<string[]> addedLineOfPronunciations = new();
			pronunciations.Add(addedLineOfPronunciations);
			foreach (string word in lineOfWords)
			{
				words += word + " ";
				var pronunciation = (await _service.GetSyllables(word));
				addedLineOfPronunciations.Add(pronunciation);
			}
		}

		string poemPronunciation = "";
		foreach (List<string[]> lineOfPronunciations in pronunciations)
		{
			foreach (string[] syllablesInWord in lineOfPronunciations)
			{
				var word = "";
				foreach (string syllable in syllablesInWord)
				{
					var syllables = syllable.Split(' ');
					var phonemes = new List<string>();
					foreach (string phoneme in syllables)
					{
						var phonemeShortened = phoneme;
						if (phonemeShortened.Length > 2)
						{
							phonemeShortened = phonemeShortened.Substring(0, 2);
						}
						phonemes.Add(phonemeShortened);

					}
					word += String.Join('-', phonemes) + ' ';
				}
				poemPronunciation += word + "/ ";
			}
			poemPronunciation = poemPronunciation.Trim();
			poemPronunciation += "\n";
		}
		return new PoemPair() { Pronunciation = poemPronunciation, Poem = words.TrimEnd() };
	}

	[HttpGet("ImperfectRhyme")]
	public async Task<List<string>> GetImperfectRhymes(string phonemesString)
	{
		return await _service.GetImperfectRhymes(phonemesString);
	}

	[HttpGet("PronunciationToPlain")]
	public async Task<List<string>> GetPronunciationToPlain(string word)
	{
		return await _service.GetPronunciationToPlain(word);
	}

	[HttpPost("AddWord")]
	public async Task<bool> PostWord(WordDto dto)
	{
		return await _service.AddWord(dto);
	}

	[HttpGet("WordListPaginated")]
	public async Task<PaginatedWordsDto> GetWordListPaginated(int countPerPage, int pageNumber)
	{
		return await _service.GetWordListPaginated(countPerPage, pageNumber);
	}
}
