namespace Rhym.Api.Requests;

public class DocumentRequest
{
	public int UserId { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
}
