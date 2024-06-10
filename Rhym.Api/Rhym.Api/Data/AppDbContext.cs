using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rhym.Api.Models;

namespace Rhym.Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Document> Documents { get; set; }
	public DbSet<DocumentData> DocumentData { get; set; }
	public DbSet<Word> Words { get; set; }
	public DbSet<Syllable> Syllables {  get; set; }
	public DbSet<Rhyme> Rhymes { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}
