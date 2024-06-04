using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Dtos;

public class WordDto
{
	public string Word { get; set; } = null!;
	public string[] Phonemes { get; set; } = null!;
	public string[] SyllablesPronunciation { get; set; } = null!;
	public string[] PlainTextSyllables { get; set; } = null!;
}
