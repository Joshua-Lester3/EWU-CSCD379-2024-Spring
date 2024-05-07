namespace Rhym.Api.Requests;

public record DocumentRequest
{
	public int UserId { get; set; }
	public string Title { get; set; } = null!;
	public string Content { get; set; } = null!;
}
