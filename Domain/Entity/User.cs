using System.ComponentModel.DataAnnotations;
using Task = ToDo.Domain.Entity.Task;
namespace ToDo.Domain.Entity
{
    public class User : BaseEntity
    {
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(200)]
        public required string Email { get; set; }
        [MaxLength(200), MinLength(8)]
        public required string Password { get; set; }

        public string? ImageUrl { get; set; }
        public ICollection<Cluster>? Clusters { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<Task>? Tasks { get; set; }

    }
}
