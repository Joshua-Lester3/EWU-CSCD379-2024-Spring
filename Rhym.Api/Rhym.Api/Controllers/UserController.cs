using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rhym.Api.Dtos;
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
		public async Task<IActionResult> AddUser(UserDto userDto)
		{
			if (string.IsNullOrEmpty(userDto.Username))
			{
				return BadRequest("Username is required");
			}
			if (string.IsNullOrEmpty(userDto.Password))
			{
				return BadRequest("Password is required");
			}
			if (string.IsNullOrEmpty(userDto.Email))
			{
				return BadRequest("Email is required");
			}

			var results = await _service.AddUser(userDto);
			switch (results.Results)
			{
				case RegistrationResults.Success:
					return Ok();
				case RegistrationResults.Failure:
					if (results.Errors is null)
					{
						return BadRequest("Something went wrong during account registration.");
					}
					else
					{
						var errors = "";
						foreach (IdentityError error in results.Errors)
						{
							errors += error.Description + '\n';
						}
						return BadRequest(errors);
					}
				default:
					return BadRequest("Account already exists.");
			}
		}
	}
}
