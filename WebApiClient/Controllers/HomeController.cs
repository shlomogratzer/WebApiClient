using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using WebApiClient.Models;

namespace WebApiClient.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		
		public IActionResult Index()
		{

			GetDataObjects();
			return View();
		}
		
		public IEnumerable<DataObject> GetDataObjects()
		{
			using var client = new HttpClient();
			client.BaseAddress = new Uri("https://dummyjson.com/todos"); // Add an Accept header for JSON format.
			client.DefaultRequestHeaders.Accept.Add(
			   new MediaTypeWithQualityHeaderValue("dummyjson.com/todos"));
			// Get data response
			var response = client.GetAsync("").Result;
			if (response.IsSuccessStatusCode)
			{
				// Parse the response body
				var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;
				foreach (var d in dataObjects)
				{
					Console.WriteLine("{0}", d.Name);
				}
			}
			else
			{
				Console.WriteLine("{0} ({1})", (int)response.StatusCode,
							  response.ReasonPhrase);
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
