using System.ComponentModel.DataAnnotations;

namespace Rhym.Api.Models;

public class Document
{
	public int DocumentId { get; set; }
	public string Title { get; set; } = null!;
	public bool Shared { get; set; } = false;

	[Required]
	public string UserId { get; set; } = null!;
	public AppUser? User { get; set; }

	[Required]
	public int DocumentDataId { get; set; }
    public DocumentData? DocumentData { get; set; }


}
