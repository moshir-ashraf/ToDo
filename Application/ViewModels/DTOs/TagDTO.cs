namespace ToDo.Application.DTO
{
    public class TagDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Priority { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }

    }
}
