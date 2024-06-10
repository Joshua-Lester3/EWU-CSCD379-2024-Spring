namespace Rhym.Api.Requests;

public record DocumentDto
{
	public string UserId { get; set; }
	public int DocumentId { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
}
