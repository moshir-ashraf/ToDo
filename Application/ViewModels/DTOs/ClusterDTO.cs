namespace ToDo.Application.DTO
{
    public class ClusterDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public ICollection<TaskDTO>? Tasks { get; set; }
    }
}
