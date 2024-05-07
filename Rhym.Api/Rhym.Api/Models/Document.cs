namespace Rhym.Api.Models;

public class Document
{
	public int DocumentId { get; set; }
	public int UserId { get; set; }
	public string Title { get; set; } = null!;

}
