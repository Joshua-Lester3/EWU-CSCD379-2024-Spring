using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Rhym.Api.Data;
using Rhym.Api.Models;
using Rhym.Api.Requests;

namespace Rhym.Api.Tests;

[TestClass]
public class DocumentControllerTests
{
	private static readonly WebApplicationFactory<Program> _factory = new();

	[TestMethod]
	public async Task GetDocumentList_HttpStatusCodeIsOK()
	{
		// Arrange
		var client = _factory.CreateClient();
		await AddOneDocument();

		// Act
		var response = await client.GetAsync("/document/getdocumentlist");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task GetDocumentList_ReturnsListSizeOne()
	{
		// Arrange
		var client = _factory.CreateClient();
		await AddOneDocument();

		// Act
		var response = await client.GetAsync("/document/getdocumentlist");
		var content = await response.Content.ReadFromJsonAsync<List<Document>>();

		// Assert
		CollectionAssert.AllItemsAreNotNull(content);
		CollectionAssert.AllItemsAreUnique(content);
		Assert.AreEqual(0, content[0].UserId);
	}

	[TestMethod]
	public async Task AddDocument_ReturnsCorrectDocument()
	{
		// Arrange

		// Act
		var response = await AddOneDocument();

		// Assert
		var document = await response.Content.ReadFromJsonAsync<Document>();
		Assert.IsNotNull(document);
		Assert.AreEqual(0, document.UserId);
		Assert.AreEqual("Super duper title", document.Title);
		Assert.AreEqual("This is super duper!", document.Content);
	}

	[TestMethod]
	public async Task AddDocument_ReturnsHttpStatusCodeOK()
	{
		// Arrange

		// Act
		var response = await AddOneDocument();

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[ClassCleanup]
	public static void ClassCleanup()
	{
		_factory.Dispose();
	}

	private static async Task<HttpResponseMessage> AddOneDocument()
	{
		var client = _factory.CreateClient();
		DocumentDto request = new()
		{
			UserId = 0,
			Title = "Super duper title",
			Content = "This is super duper!"
		};
		var content = JsonContent.Create(request);
		return await client.PostAsync("/document/adddocument", content);
	}
}
