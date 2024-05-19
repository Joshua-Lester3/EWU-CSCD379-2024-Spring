﻿using Microsoft.EntityFrameworkCore;
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
			Where(dbDocument => dbDocument.UserId == request.UserId && dbDocument.Title == request.Title).
			Include(document => document.DocumentData).
			FirstOrDefaultAsync(); //DocumentData will never be null here
		if (foundDocument is null)
		{
			lock (_addingDocumentLock)
			{
				foundDocument = _context.Documents.
					Include(dbDocument => dbDocument.DocumentData).
					FirstOrDefault(dbDocument => dbDocument.UserId == request.UserId && dbDocument.Title == request.Title); //DocumentData will never be null here
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
					foundDocument.Title = request.Title; //DocumentData will never be null here
					foundDocument.DocumentData!.Content = request.Content; //DocumentData will never be null here
					_context.SaveChanges();
					return foundDocument;
				}
			}
		}
		else
		{
			lock (_changingDocumentLock)
			{
				foundDocument.Title = request.Title; //DocumentData will never be null here
				foundDocument.DocumentData!.Content = request.Content; //DocumentData will never be null here
				_context.SaveChanges();
				return foundDocument;
			}
		}
	}

	public async Task<DocumentData?> GetDocumentDataAsync(int documentId)
	{
		var result = _context.Documents
			.Where(document => document.DocumentId == documentId)
			.Include(document => document.DocumentData)
			.FirstOrDefault()?
			.DocumentData;
		return result;
	}
}