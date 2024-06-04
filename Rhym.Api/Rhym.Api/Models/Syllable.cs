using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Syllable
{
	public int SyllableId { get; set; }
	[Required]
	public required string WordKey { get; set; }
	[Required]
	public int WordId {  get; set; }
	public Word? Word { get; set; }
	[Required]
	public required string[] PlainTextSyllables { get; set; }

}
