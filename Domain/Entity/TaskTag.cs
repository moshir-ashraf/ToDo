using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Domain.Entity
{
    public class TaskTag
    {

        public required int TaskId { get; set; }
        public required int TagId { get; set; }

        [ForeignKey("TaskId")]
        public Task? Task { get; set; }
        [ForeignKey("TagId")]
        public Tag? Tag { get; set; }
    }
}