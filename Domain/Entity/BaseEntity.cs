using System.ComponentModel.DataAnnotations;

namespace ToDo.Domain.Entity
{
    public class BaseEntity
    {
        [Key]
        public required int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
