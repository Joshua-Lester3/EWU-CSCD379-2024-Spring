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
	private static object _deletingDocumentLock = new();
	private static object _togglingSharedLock = new();
	public DocumentService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<Document>> GetDocumentListAsync(string userId)
	{
		return await _context.Documents.Where(document => document.UserId == userId).ToListAsync();
	}

	public async Task<Document> PostDocumentAsync(DocumentDto request)
	{
		Document? foundDocument = null;
		if (request.DocumentId != -1)
		{
			foundDocument = await _context.Documents.
				Where(dbDocument => dbDocument.DocumentId == request.DocumentId).
				Include(document => document.DocumentData).
				FirstOrDefaultAsync();
		}
		if (foundDocument is null)
		{
			lock (_addingDocumentLock)
			{
				if (request.DocumentId != -1)
				{
					foundDocument = _context.Documents.
						Include(dbDocument => dbDocument.DocumentData).
						FirstOrDefault(dbDocument => dbDocument.DocumentId == request.DocumentId);
				}

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
						Shared = request.IsShared,
					};
					_context.Documents.Add(addedDocument);
					_context.SaveChanges();
					return addedDocument;
				}
				else
				{
					var foundDocumentData = _context.DocumentData.FirstOrDefault(documentData => documentData == foundDocument.DocumentData);
					if (foundDocumentData is null)
					{
						throw new InvalidOperationException("Invalid state: No DocumentData for the corresponding Document entity.");
					}
					foundDocumentData.Content = request.Content;
					foundDocument.Title = request.Title;
					foundDocument.Shared = request.IsShared;
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
				if (foundDocumentData is null)
				{
					throw new InvalidOperationException("Invalid state: No DocumentData for the corresponding Document entity.");
				}
				foundDocumentData.Content = request.Content;
				foundDocument.Title = request.Title;
				foundDocument.Shared = request.IsShared;
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
				IsShared = document.Shared,
			})
			.FirstOrDefaultAsync();
		return result;
	}

	public async Task<bool> DeleteDocumentAsync(int documentId)
	{
		var foundDocument = await _context.Documents.FirstOrDefaultAsync(document => document.DocumentId == documentId);
		if (foundDocument is not null)
		{
			lock (_deletingDocumentLock)
			{
				foundDocument = _context.Documents.Include(document => document.DocumentData).FirstOrDefault(document => document.DocumentId == documentId);
				if (foundDocument is not null)
				{
					_context.Documents.Remove(foundDocument);
					var documentData = foundDocument.DocumentData;
					if (documentData is not null)
					{
						_context.DocumentData.Remove(documentData);
					}
					_context.SaveChanges();
					return true;
				}
				return false;
			}
		}
		return false;
	}

	public async Task<Document?> ToggleSharedAsync(int documentId, bool isShared)
	{
		var foundDocument = await _context.Documents.FirstOrDefaultAsync(document => document.DocumentId == documentId);
		if (foundDocument is not null)
		{
			lock (_togglingSharedLock)
			{
				foundDocument = _context.Documents.FirstOrDefault(document => document.DocumentId == documentId);
				if (foundDocument is not null)
				{
					foundDocument.Shared = isShared;
					_context.SaveChanges();
				}
				return foundDocument;
			}
		}
		return null;
	}
}
