using Microsoft.AspNetCore.Mvc;
using Rhym.Api.Models;

namespace Rhym.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
	[HttpGet("GetDocumentList")]
	public List<Document> GetDocumentList()
	{
		return new List<Document> {
			new Document()
			{
				DocumentId = 0,
				UserId = 0,
				Title = "Document, YAY!"
			},
			new Document()
			{
				DocumentId = 1,
				UserId = 1,
				Title = "Document2, YAY!"
			}
			};
	}
}
