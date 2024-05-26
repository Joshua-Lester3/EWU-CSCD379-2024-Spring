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

	[HttpPost("PoemPronunciation")]
	public async Task<string> GetPoemPronunciation(DocumentDataDto poem)
	{
		var lines = poem.Content.Split(Environment.NewLine);
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

	[HttpGet("ImperfectRhyme")]
	public async Task<List<string>> GetImperfectRhymes(string phonemesString)
	{
		return await _service.GetImperfectRhymes(phonemesString);
	}
}
