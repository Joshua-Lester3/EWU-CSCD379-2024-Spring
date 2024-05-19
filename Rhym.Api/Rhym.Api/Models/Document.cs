using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Document
{
	public int DocumentId { get; set; }
	public string Title { get; set; } = null!;

	[Required]
	public int UserId { get; set; }
	public User? User { get; set; }

	[Required]
	public int DocumentDataId { get; set; }
    public DocumentData? DocumentData { get; set; }


}
