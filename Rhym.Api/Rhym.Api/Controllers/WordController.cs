﻿using Microsoft.AspNetCore.Mvc;
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

	[HttpGet("ImperfectRhyme")]
	public async Task<List<string>> GetImperfectRhymes(string phonemesString)
	{
		return await _service.GetImperfectRhymes(phonemesString);
	}
}
