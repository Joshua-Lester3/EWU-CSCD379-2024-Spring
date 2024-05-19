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

	public async Task<List<Document>> GetDocumentListAsync(int userId)
	{
		return await _context.Documents.Where(document => document.UserId == userId).ToListAsync();
	}

	public async Task<Document> AddDocumentAsync(DocumentDto request)
	{
		Document? foundDocument = await _context.Documents.
			Where(dbDocument => dbDocument.DocumentId == request.DocumentId).
			Include(document => document.DocumentData).
			FirstOrDefaultAsync();
		if (foundDocument is null)
		{
			lock (_addingDocumentLock)
			{
				foundDocument = _context.Documents.
					Include(dbDocument => dbDocument.DocumentData).
					FirstOrDefault(dbDocument => dbDocument.DocumentId == request.DocumentId);
				if (foundDocument is null)
				{
					DocumentData data = new DocumentData
					{
						Content = request.Content,
					};
					_context.DocumentData.Add(data);
					_context.SaveChanges();
					Document addedDocument = new Document
					{
						UserId = request.UserId,
						DocumentData = data,
						Title = request.Title,
					};
					_context.Documents.Add(addedDocument);
					_context.SaveChanges();
					return addedDocument;
				} else
				{
					var foundDocumentData = _context.DocumentData.FirstOrDefault(documentData => documentData == foundDocument.DocumentData);
					if (foundDocumentData is null)
					{
						throw new InvalidOperationException("Invalid state: No DocumentData for the corresponding Document entity.");
					}
					foundDocumentData.Content = request.Content;
					//foundDocument.DocumentData!.Content = request.Content; //DocumentData will never be null here
					foundDocument.Title = request.Title;
					_context.SaveChanges();
					return foundDocument;
				}
			}
		}
		else
		{
			lock (_changingDocumentLock)
			{
				var foundDocumentData = _context.DocumentData.FirstOrDefault(documentData => documentData == foundDocument.DocumentData);
				if (foundDocumentData is null) {
					throw new InvalidOperationException("Invalid state: No DocumentData for the corresponding Document entity.");
				}
				foundDocumentData.Content = request.Content;
				//foundDocument.DocumentData!.Content = request.Content; //DocumentData will never be null here
				foundDocument.Title = request.Title;
				_context.SaveChanges();
				return foundDocument;
			}
		}
	}

	public async Task<DocumentDto?> GetDocumentDataAsync(int documentId)
	{
		var result = await _context.Documents
			.Where(document => document.DocumentId == documentId)
			.Include(document => document.DocumentData)
			.Select(document => new DocumentDto
			{
				UserId = document.UserId,
				DocumentId = document.DocumentId,
				Title = document.Title,
				Content = document.DocumentData!.Content,
			})
			.FirstOrDefaultAsync();
		return result;
	}
}
