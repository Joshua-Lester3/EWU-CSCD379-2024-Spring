using Microsoft.AspNetCore.Identity;
using Rhym.Api.Data;
using Rhym.Api.Models;

namespace Rhym.Api.Identity;

public class IdentitySeed
{
	public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
	{
		// Seed Roles
		await SeedRolesAsync(roleManager);

		// Seed Admin User
		await SeedAdminUserAsync(userManager);
	}

	private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
	{
		// Seed Roles
		if (!await roleManager.RoleExistsAsync(Roles.Admin))
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
		}
	}

	private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
	{
		// Seed Admin User
		if (await userManager.FindByEmailAsync("admin@ewu.edu") == null)
		{
			AppUser user = new AppUser
			{
				UserName = "admin@ewu.edu",
				Email = "admin@ewu.edu"
			};

			IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, Roles.Admin);
			}
		}
	}
}
