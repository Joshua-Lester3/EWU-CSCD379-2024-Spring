
using Rhym.Api.Data;
using Rhym.Api.Models;
using Rhym.Api.Services;
using Wordle.Api.Tests;

namespace Rhym.Api.Tests;

[TestClass]
public class WordServiceTests : DatabaseTestBase
{
	private WordService _service = null!; // Both will be initialized in TestInitialize
	private AppDbContext _context = null!;

	//[TestInitialize]
	//public async Task Init()
	//{
	//	_context = new AppDbContext(Options);
	//	_service = new(_context);
	//	await _context.Words.AddAsync(new Word { WordKey = "ABET", Phonemes = ["AH0", "B", "EH1", "T"], SyllablesPronunciation = ["AH0", "B EH1 T"] });
	//	await _context.Words.AddAsync(new Word { WordKey = "BABETTE", Phonemes = ["B", "AH0", "B", "EH1", "T"], SyllablesPronunciation = ["B AH0", "B EH1 T"] });
	//	await _context.Words.AddAsync(new Word { WordKey = "BET", Phonemes =[ "B", "EH1", "T"], SyllablesPronunciation = ["B EH1 T"] });
	//	await _context.Words.AddAsync(new Word { WordKey = "BETA", Phonemes = ["B", "EY1", "T", "AH0"], SyllablesPronunciation = ["B EY1", "T AH0"] });
	//	await _context.SaveChangesAsync();
	//}

	//[TestMethod]
	//public async Task GetPerfectRhymes_Success()
	//{
	//	// Arrange
	//	string word = "ABET";

	//	// Act
	//	var perfectRhymes = await _service.GetPerfectRhymes(word);

	//	// Assert
	//	Assert.AreEqual(1, perfectRhymes.Count());
	//	CollectionAssert.Contains(perfectRhymes, "BABETTE");
	//}

	//[TestCleanup]
	//public void Cleanup()
	//{
	//	_context.Dispose();
	//}
}
