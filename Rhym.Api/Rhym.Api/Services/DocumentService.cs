using Microsoft.EntityFrameworkCore;
using Rhym.Api.Data;
using Rhym.Api.Models;
using Rhym.Api.Requests;

namespace Rhym.Api.Services;

public class DocumentService
{
	private readonly AppDbContext _context;
	private static object _changingDocumentLock = new();
	private static object _addingDocumentLock = new();
	public DocumentService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<Document>> GetDocumentList(int userId)
	{
		return await _context.Documents.Where(document => document.UserId == userId).ToListAsync();
	}

	public async Task<Document> AddDocument(DocumentDto request)
	{
		Document? foundDocument = await _context.Documents.
			FirstOrDefaultAsync(dbDocument => dbDocument.UserId == request.UserId && dbDocument.Title == request.Title);
		if (foundDocument is null)
		{
			lock (_addingDocumentLock)
			{
				foundDocument = _context.Documents.
					FirstOrDefault(dbDocument => dbDocument.UserId == request.UserId && dbDocument.Title == request.Title);
				if (foundDocument is null)
				{
					Document addedDocument = new Document
					{
						UserId = request.UserId,
						Title = request.Title,
						Content = request.Content
					};
					_context.Documents.Add(addedDocument);
					_context.SaveChanges();
					return addedDocument;
				} else
				{
					foundDocument.Title = request.Title;
					foundDocument.Content = request.Content;
					_context.SaveChanges();
					return foundDocument;
				}
			}
		}
		else
		{
			lock (_changingDocumentLock)
			{
				foundDocument.Title = request.Title;
				foundDocument.Content = request.Content;
				_context.SaveChanges();
				return foundDocument;
			}
		}
	}
}
