using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Domain.Entity
{
    public class Task : BaseEntity
    {
        [MaxLength(200)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required bool IsComplete { get; set; } = false;
        public required int UserId { get; set; }
        public required int ClusterId { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("ClusterId")]
        public Cluster? Cluster { get; set; }

        public ICollection<TaskTag>? TaskTags { get; set; } = new List<TaskTag>();
    }
}
