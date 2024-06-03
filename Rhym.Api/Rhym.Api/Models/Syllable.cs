using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Syllable
{
	public int SyllableId { get; set; }
	[Required]
	public required string Word { get; set; }
	[Required]
	public required int WordId {  get; set; }
	[Required]
	public required string[] Syllables { get; set; }

}
