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
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = _factory.CreateClient();
	}

	[TestMethod]
	public async Task GetDocumentList_HttpStatusCodeIsOK()
	{
		// Arrange
		await AddOneDocument();

		// Act
		var response = await _httpClient.GetAsync("/document/getdocumentlist");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task GetDocumentList_ReturnsListSizeOne()
	{
		// Arrange
		await AddOneDocument();
		var queryParameters = new Dictionary<string, string>
		{
			{ "userId", "1" },
		};
		var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
		var queryString = await dictFormUrlEncoded.ReadAsStringAsync();


		// Act
		var response = await _httpClient.GetAsync($"/document/getdocumentlist?{queryString}");
		var content = await response.Content.ReadFromJsonAsync<List<Document>>();

		// Assert
		CollectionAssert.AllItemsAreNotNull(content);
		CollectionAssert.AllItemsAreUnique(content);
		Assert.AreEqual(1, content.Count());
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

	[TestMethod]
	public async Task GetDocumentData_ReturnsHttpStatusCodeOK()
	{
		// Arrange
		await AddOneDocument();

		// Act
		var response = await _httpClient.GetAsync("/document/getdocumentdata");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task GetDocumentData_ReturnsCorrectDocumentTitleAndContent()
	{
		// Arrange
		var addResponse = await AddOneDocument();
		var document = await addResponse.Content.ReadFromJsonAsync<Document>();

		// Act
		var response = await _httpClient.GetAsync("/document/getdocumentdata");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[ClassCleanup]
	public static void ClassCleanup()
	{
		_factory.Dispose();
	}

	private async Task<HttpResponseMessage> AddOneDocument()
	{
		DocumentDto request = new()
		{
			UserId = 0,
			Title = "Super duper title",
			Content = "This is super duper!"
		};
		var content = JsonContent.Create(request);
		return await _httpClient.PostAsync("/document/adddocument", content);
	}
}
