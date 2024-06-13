using Microsoft.AspNetCore.Mvc;
using Rhym.Api.Dtos;
using Rhym.Api.Models;
using Rhym.Api.Services;

namespace Rhym.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WordController : ControllerBase
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

	[HttpGet("GetWord")]
	public async Task<Rhyme?> GetWord(string word)
	{
		return await _service.GetWord(word);
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
	public async Task<IActionResult> PostWord(WordDto dto)
	{
		if (string.IsNullOrEmpty(dto.Word))
		{
			return BadRequest("Word is empty.");
		}
		if (dto.SyllablesPronunciation.Length == 0)
		{
			return BadRequest("No pronunciation given.");
		}
		if (dto.PlainTextSyllables.Length == 0)
		{
			return BadRequest("No plaintext given.");
		}
		if (dto.SyllablesPronunciation.Length != dto.PlainTextSyllables.Length)
		{
			return BadRequest("Number of syllables for pronunciation and plaintext are not equal to each other.");
		}

		if (await _service.AddWord(dto))
		{
			return Ok();
		}
		return BadRequest("Word already in dictionary.");
	}

	[HttpGet("WordListPaginated")]
	public async Task<PaginatedWordsDto> GetWordListPaginated(int countPerPage, int pageNumber, string? word = null)
	{
		return await _service.GetWordListPaginated(countPerPage, pageNumber, word);
	}
}
