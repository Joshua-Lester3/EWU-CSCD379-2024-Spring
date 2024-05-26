using Rhym.Api.Services;
using Rhym.Api.Data;
using Wordle.Api.Tests;
using Rhym.Api.Requests;
using Rhym.Api.Models;
using Rhym.Api.Dtos;

namespace Rhym.Api.Tests;

[TestClass]
public class DocumentServiceTests : DatabaseTestBase
{
	private DocumentService _documentService = null!;
	private UserService _userService = null!;
	private AppDbContext _context = null!;

	[TestInitialize]
	public async Task Init()
	{
		_context = new AppDbContext(Options);
		_documentService = new(_context);
		_userService = new UserService(_context);
		await _userService.AddUser(new UserDto { Name = "Jimbob" });
	}

	[TestMethod]
	public async Task AddDocument_SuccessfullyAddsDocument()
	{
		// Arrange
		DocumentDto request = new()
		{
			UserId = 1,
			Title = "Super duper title",
			Content = "This doc is super duper!"
		};

		// Act
		Document document = await _documentService.PostDocumentAsync(request);

		// Assert
		Assert.AreEqual(1, document.UserId);
		CollectionAssert.Contains(_context.Documents.ToList(), document);
	}
	
	[TestMethod]
	public async Task GetDocumentList_ReturnsPostedDocuments()
	{
		// Arrange
		DocumentDto requestOne = new()
		{
			UserId = 1,
			Title = "Super duper title",
			Content = "This doc is super duper!"
		};
		DocumentDto requestTwo = new()
		{
			UserId = 1,
			Title = "Super duper title 2",
			Content = "This doc is super duper times two!"
		};
		await _documentService.PostDocumentAsync(requestOne);
		await _documentService.PostDocumentAsync(requestTwo);

		// Act
		List<Document> documents = await _documentService.GetDocumentListAsync(1);

		// Assert
		Assert.AreEqual(2, documents.Count);
	}

	[TestCleanup] 
	public void Cleanup()
	{
		_context.Dispose();
	}
}
