using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using WebApiClient.Models;

namespace WebApiClient.Controllers
{
    public class TodoController : Controller
    {
        static HttpClient _httpClient = new HttpClient();
        public TodoController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult CreateTodo(int UserId)
        {
            Todos todos = new Todos() { UserId = UserId };
            return View(todos);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTodo(Todos todo)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7177/api/Todo/AddMission", todo);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to a different action, such as the list of users
                    return RedirectToAction("DetailsOfAll");
                }
                else
                {
                    // Handle the case where the creation failed
                    ModelState.AddModelError("", "Failed to create user.");
                }
            }

            // If we reach this point, something went wrong; return the view with the user data
            return RedirectToAction("DetailsOfAll");


        }
    }
}
