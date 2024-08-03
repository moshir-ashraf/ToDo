namespace ToDo.Application.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int ClusterId { get; set; }
        public ICollection<TagDTO>? Tags { get; set; }
    }
}
