using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Rhym.Api.Models;

namespace Rhym.Api.Tests;

[TestClass]
public class DocumentControllerTests
{
	private readonly WebApplicationFactory<Program> _factory = new();

	[TestMethod]
	public async Task GetDocumentList_HttpStatusCodeIsOK()
	{
		// Arrange
		var client = _factory.CreateClient();

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

		// Act
		var response = await client.GetAsync("/document/getdocumentlist");
		var content = await response.Content.ReadFromJsonAsync<List<Document>>();

		// Assert
		CollectionAssert.AllItemsAreNotNull(content);
		CollectionAssert.AllItemsAreUnique(content);
		Assert.AreEqual(0, content[0].UserId);
	}
}
