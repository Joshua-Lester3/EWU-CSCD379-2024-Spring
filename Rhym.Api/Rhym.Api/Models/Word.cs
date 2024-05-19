using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Word
{
	public int Id { get; set; }

	[Required]
	public required string WordKey { get; set; }

	[Required]
	public required string Pronunciation { get; set; }
}
