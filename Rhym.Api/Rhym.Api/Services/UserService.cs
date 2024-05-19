using Microsoft.EntityFrameworkCore;
using Rhym.Api.Data;
using Rhym.Api.Dtos;
using Rhym.Api.Models;

namespace Rhym.Api.Services;

public class UserService
{
	private readonly AppDbContext _context;

	public UserService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<User> AddUser(UserDto userDto)
	{
		User user = new User
		{
			Name = userDto.Name,
		};
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();
		return user;
	}
}
