using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Rhym.Api.Identity;

public class Policies
{
	public const string Admin = "Admin";

	public static void AdminPolicy(AuthorizationPolicyBuilder policy)
	{
		policy.RequireRole(Roles.Admin);
	}
}
