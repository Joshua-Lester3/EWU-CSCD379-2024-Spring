using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Word
{
	public int WordId { get; set; }

	[Required]
	public required string WordKey { get; set; }

	[Required]
	public required string[] Phonemes { get; set; }

	[Required]
	public required string[] SyllablesPronunciation { get; set; }
	public Syllable? Syllable { get; set; }
}
