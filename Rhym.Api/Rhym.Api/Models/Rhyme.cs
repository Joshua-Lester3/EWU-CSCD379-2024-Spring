using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Rhyme
{
	public int RhymeId { get; set; }
	[Required]
	public required string Word { get; set; }
	[Required]
	public required string[] Phonemes { get; set; }

	[Required]
	public required string[] SyllablesPronunciation { get; set; }
	[Required]
	public required string[] PlainTextSyllables { get; set; }
}
