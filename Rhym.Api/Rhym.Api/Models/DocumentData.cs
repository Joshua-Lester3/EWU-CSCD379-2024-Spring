namespace Rhym.Api.Models;

public class DocumentData
{
    public int DocumentDataId { get; set; }
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
