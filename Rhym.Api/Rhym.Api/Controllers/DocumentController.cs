using Microsoft.AspNetCore.Mvc;
using Rhym.Api.Models;
using Rhym.Api.Requests;
using Rhym.Api.Services;

namespace Rhym.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
	private readonly DocumentService _service;
	public DocumentController(DocumentService service)
	{
		_service = service;
	}

	[HttpGet("GetDocumentList")]
	public async Task<List<Document>> GetDocumentListAsync(int userId)
	{
		return await _service.GetDocumentList(userId);
	}

	[HttpPost("AddDocument")]
	public async Task<Document> AddDocumentAsync(DocumentDto dto)
	{
		return await _service.AddDocument(dto);
	}

	[HttpGet("GetDocument")]
	public async Task<Document> GetDocumentAsync(int documentId)
	{
		return await _service.GetDocumentDataAsync(documentId);
	}
}
