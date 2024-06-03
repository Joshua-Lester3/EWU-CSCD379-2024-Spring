using Microsoft.EntityFrameworkCore;
using Rhym.Api.Models;

namespace Rhym.Api.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Document> Documents { get; set; }
	public DbSet<DocumentData> DocumentData { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Word> Words { get; set; }
	public DbSet<Syllable> Syllables {  get; set; }
}
