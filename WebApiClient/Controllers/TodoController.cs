using Microsoft.AspNetCore.Mvc;

namespace WebApiClient.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
