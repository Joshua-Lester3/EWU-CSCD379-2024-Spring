using Rhym.Api.Services;
using Rhym.Api.Data;
using Wordle.Api.Tests;
using Rhym.Api.Requests;
using Rhym.Api.Models;

namespace Rhym.Api.Tests;

[TestClass]
public class DocumentServiceTests : DatabaseTestBase
{
	private DocumentService _service = null!;
	private AppDbContext _context = null!;

	[TestInitialize]
	public void Init()
	{
		_context = new AppDbContext(Options);
		_service = new(_context);
	}

	[TestMethod]
	public async Task AddDocument_SuccessfullyAddsDocument()
	{
		// Arrange
		DocumentRequest request = new()
		{
			UserId = 0,
			Title = "Super duper title",
			Content = "This doc is super duper!"
		};

		// Act
		Document document = await _service.AddDocument(request);

		// Assert
		Assert.AreEqual(0, document.UserId);
		Assert.AreEqual("Super duper title", document.Title);
		Assert.AreEqual("This doc is super duper!", document.Content);
		CollectionAssert.Contains(_context.Documents.ToList(), document);
	}
	
	[TestMethod]
	public async Task GetDocumentList_ReturnsPostedDocuments()
	{
		// Arrange
		DocumentRequest requestOne = new()
		{
			UserId = 0,
			Title = "Super duper title",
			Content = "This doc is super duper!"
		};
		DocumentRequest requestTwo = new()
		{
			UserId = 0,
			Title = "Super duper title 2",
			Content = "This doc is super duper times two!"
		};
		await _service.AddDocument(requestOne);
		await _service.AddDocument(requestTwo);

		// Act
		List<Document> documents = await _service.GetDocumentList(0);

		// Assert
		Assert.AreEqual(2, documents.Count);
	}
}
