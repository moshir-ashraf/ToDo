using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Domain.Entity
{
    public class Cluster : BaseEntity
    {
        [MaxLength(200)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";

        public required int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public List<Task>? Tasks { get; set; } = new List<Task>();
    }
}
