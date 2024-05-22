using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Word
{
	public int Id { get; set; }

	[Required]
	public required string WordKey { get; set; }

	[Required]
	public required string[] Phonemes { get; set; }

	[Required]
	public required string[] Syllables { get; set; }
}
