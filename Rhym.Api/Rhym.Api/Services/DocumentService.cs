using Rhym.Api.Data;
using Rhym.Api.Models;

namespace Rhym.Api.Services;

public class DocumentService
{
	private readonly AppDbContext _context;
	public DocumentService(AppDbContext context)
	{
		_context = context;
	}

	public List<Document> GetDocumentList(int userId)
	{
		return null!;
	}
}
