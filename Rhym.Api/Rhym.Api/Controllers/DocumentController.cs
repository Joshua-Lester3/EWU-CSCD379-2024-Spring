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
	public async Task<List<Document>> GetDocumentList(int userId)
	{
		return await _service.GetDocumentList(userId);
	}

	[HttpPost("AddDocument")]
	public async Task<Document> AddDocument(DocumentRequest request)
	{
		return await _service.AddDocument(request);
	}
}
