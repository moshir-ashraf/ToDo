using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Domain.Entity
{
    public class Tag : BaseEntity
    {
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(50)]
        public string? Priority { get; set; }

        public required int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<TaskTag> TagTasks { get; set; } = new List<TaskTag>();
    }
}
