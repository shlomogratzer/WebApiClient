using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiClient.Models
{
    public class Todos
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
