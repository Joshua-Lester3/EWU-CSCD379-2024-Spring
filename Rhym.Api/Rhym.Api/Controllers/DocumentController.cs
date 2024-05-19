using Microsoft.AspNetCore.Mvc;
using Rhym.Api.Models;
using Rhym.Api.Requests;
using Rhym.Api.Services;
using Rhym.Api.Dtos;

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
		return await _service.GetDocumentListAsync(userId);
	}

	[HttpPost("AddDocument")]
	public async Task<Document> AddDocumentAsync(DocumentDto dto)
	{
		return await _service.AddDocumentAsync(dto);
	}

	[HttpGet("GetDocumentData")]
	public async Task<DocumentDto?> GetDocumentAsync(int documentId)
	{
		return await _service.GetDocumentDataAsync(documentId);
	}
}
