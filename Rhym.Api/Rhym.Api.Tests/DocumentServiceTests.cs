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
		DocumentDto request = new()
		{
			UserId = 0,
			Title = "Super duper title",
			Content = "This doc is super duper!"
		};

		// Act
		Document document = await _service.AddDocumentAsync(request);

		// Assert
		Assert.AreEqual(0, document.UserId);
		CollectionAssert.Contains(_context.Documents.ToList(), document);
	}
	
	[TestMethod]
	public async Task GetDocumentList_ReturnsPostedDocuments()
	{
		// Arrange
		DocumentDto requestOne = new()
		{
			UserId = 0,
			Title = "Super duper title",
			Content = "This doc is super duper!"
		};
		DocumentDto requestTwo = new()
		{
			UserId = 0,
			Title = "Super duper title 2",
			Content = "This doc is super duper times two!"
		};
		await _service.AddDocumentAsync(requestOne);
		await _service.AddDocumentAsync(requestTwo);

		// Act
		List<Document> documents = await _service.GetDocumentListAsync(0);

		// Assert
		Assert.AreEqual(2, documents.Count);
	}

	[TestCleanup] 
	public void Cleanup()
	{
		_context.Dispose();
	}
}
