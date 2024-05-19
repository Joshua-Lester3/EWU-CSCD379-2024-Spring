using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rhym.Api.Data;
using Rhym.Api.Dtos;
using Rhym.Api.Models;
using Rhym.Api.Services;

namespace Rhym.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserService _service;

		public UserController(UserService service)
		{
			_service = service;
		}

		[HttpPost("AddUser")]
		public async Task<User> AddUser(UserDto userDto)
		{
			return await _service.AddUser(userDto);
		}
	}
}
