namespace ToDo.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<ClusterDTO>? Clusters { get; set; }
        public ICollection<TagDTO>? Tags { get; set; }
    }
}
