namespace WebApiClient.Models
{
	public class Todos
	{
		public string Id { get; set; }
		public string Todo { get; set; }
		public bool Completed { get; set; }
		public int UserId { get; set; }
	}
}
