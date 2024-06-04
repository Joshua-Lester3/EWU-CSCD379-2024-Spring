namespace Rhym.Api.Dtos;

public class PaginatedWordsDto
{
    public required List<WordDto> Words { get; set; }
    public required int Pages { get; set; }
}
