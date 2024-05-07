using Microsoft.AspNetCore.Mvc;

namespace Rhym.Api.Controllers
{
	public class DocumentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
