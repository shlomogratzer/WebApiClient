using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using WebApiClient.Models;
using System.Net.Http.Json;

namespace WebApiClient.Controllers
{
	public class UsersController : Controller
	{
        /*private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}*/

        

        static HttpClient _httpClient = new HttpClient();
        public UsersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: HomeController1/Details/5
        [HttpGet]
        public async Task<IActionResult> DetailsOfAll()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7177/api/User/users");
            string content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<User>>(content);
            
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7177/api/Todo/TodosByUser/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<List<Todos>>(content);
                ViewData["UserId"] = id;
                return View(item);
            }
            else
            {
                // Handle the case where the user is not found or the request failed
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7177/api/User/users/{id}");
            string content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<User>(content);
            //ViewData["Id"] = id;
            return View(items);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(int Id, UserDto user)
        {
            if (ModelState.IsValid)
            {
                // Send the PUT request to the API
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7177/api/User/users/{Id}", user);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to a different action (e.g., details view of the updated user)
                    return RedirectToAction("DetailsOfAll");
                }
                else
                {
                    // Handle the case where the update failed
                    ModelState.AddModelError("", "Failed to update user.");
                }
            }

            // If we reach this point, something went wrong; return the view with the user data
            return View(user);
        }
        
        public async Task<IActionResult> DeleteUser(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"https://localhost:7177/api/User/users/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Redirect to the index or another list view after successful deletion
                return RedirectToAction("DetailsOfAll");
            }
            else
            {
                // Handle the case where the deletion failed
                ModelState.AddModelError("", "Failed to delete user.");
                // Optionally, you could return a view that shows an error message
                return RedirectToAction("DetailsOfAll");
            }
        }
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7177/api/User", user);

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

        //----------------------------------------------
        
        public async Task<IActionResult> Index()
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync("http://localhost:7177/api/Todo/Todo");
				if (response.IsSuccessStatusCode)
				{
					string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    List<Todos> todos = JsonConvert.DeserializeObject<List<Todos>>(jsonObject.todos.ToString());
                    return View(todos);
                }
			}

			return View("Error");
		}

        public async Task<IActionResult> UpdateTodo(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://dummyjson.com/todos");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (jsonObject != null)
                    {
                        foreach (var item in jsonObject.todos)
                        {
                            if (item != null && item.id == id)
                            {
                                Tododto todos = JsonConvert.DeserializeObject<Tododto>(item.ToString());
                                return View(todos);
                            }
                        }
                    }
                }
            }
            return View("Error");
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateTodo(int id,Todos tododto)
        {
            using (HttpClient client = new HttpClient())
            {
                // Create the updated object
                Todos updatedTodo = new Todos()
                {
                    Status = tododto.Status,
                };

                // Convert the object to JSON
                string json = JsonConvert.SerializeObject(updatedTodo);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the PUT request to update the object
                HttpResponseMessage response = await client.PutAsync($"https://dummyjson.com/todos/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally, retrieve the updated object or just redirect to the index page
                    string updatedJson = await response.Content.ReadAsStringAsync();
                    var updatedTodoItem = JsonConvert.DeserializeObject<Todos>(updatedJson);
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle errors appropriately
                    ViewBag.Error = $"Failed to update the todo. Status code: {response.StatusCode}";
                    return View("Error");
                }
            }
        }




        public IActionResult Privacy()
		{
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
