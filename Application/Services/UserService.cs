using Application.IServices;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ToDo.Application.DTO;
using ToDo.Domain.Entity;
using ToDo.Infrastructure.Data;
namespace ToDo.Application.Services
{
    public class UserService : IDTOMapper<User, UserDTO>, IUserService
    {
        private readonly AppDbContext _context;
        private readonly ClusterService _clusterService;
        private readonly TagService _tagService;
        public UserService(AppDbContext context, ClusterService clusterService, TagService tagService)
        {
            _context = context;
            _clusterService = clusterService;
            _tagService = tagService;
        }

        public UserDTO MapToDTO(User data)
        {
            var clusters = data.Clusters
                    .Select(x => _clusterService.MapToDTO(x))
                    .ToList();

            var tags = data.Tags
                    .Select(x => _tagService.MapToDTO(x))
                    .ToList();

            return new UserDTO
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Password = new string('*', data.Password.Length),
                ImageUrl = data.ImageUrl ?? string.Empty,
                Clusters = clusters,
                Tags = tags
            };
        }

        public User MapToEntity(UserDTO data)
        {
            return new User
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Password = data.Password,
                ImageUrl = data.ImageUrl ?? string.Empty,
                Clusters = data.Clusters.Select(x => _clusterService.MapToEntity(x)).ToList(),
                Tags = data.Tags.Select(x => _tagService.MapToEntity(x)).ToList()
            };
        }

        public async Task<ResultViewModel<UserDTO>> Create(User data)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Id == data.Id || x.Email == data.Email) != null)
                return new ResultViewModel<UserDTO>(null, false, "User already exists");

            await _context.Users.AddAsync(data);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(data), true, "User created successfully");
        }

        public async Task<ResultViewModel<UserDTO>> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return new ResultViewModel<UserDTO>(null, false, "User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "User deleted successfully");
        }

        public async Task<ResultViewModel<UserDTO>> Get(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return new ResultViewModel<UserDTO>(null, false, "User not found");

            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "User retrieved successfully");
        }

        public async Task<ResultViewModel<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return new ResultViewModel<IEnumerable<UserDTO>>(new List<UserDTO>(), false, "No users found");

            var usersDTO = users.Select(x => MapToDTO(x));
            return new ResultViewModel<IEnumerable<UserDTO>>(usersDTO, true, "Users retrieved successfully");
        }

        public async Task<ResultViewModel<UserDTO>> Update(User data, int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return new ResultViewModel<UserDTO>(null, false, "User not found");

            if (await _context.Users.FirstOrDefaultAsync(x => x.Email == data.Email) != null)
                return new ResultViewModel<UserDTO>(null, false, "Email already exists");

            user.Name = data.Name;
            user.Email = data.Email;
            user.ImageUrl = data.ImageUrl ?? string.Empty;
            if(data.Password != null)   user.Password = data.Password;
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "User updated successfully");
        }

        public async Task<ResultViewModel<UserDTO>> GetByCredentials(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (user == null)
                return new ResultViewModel<UserDTO>(null, false, "Invalid email or password");

            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "User logged in successfully");
        }

        public async Task<ResultViewModel<UserDTO>> AddCluster(User user, Cluster cluster)
        {
           if(user.Clusters.Any(c=> c.Id == cluster.Id || c.Name == cluster.Name))
                return new ResultViewModel<UserDTO>(MapToDTO(user), false, "Cluster already exists");

            user.Clusters.Add(cluster);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "Cluster added successfully");
        }

        public async Task<ResultViewModel<UserDTO>> RemoveCluster(User user, Cluster cluster)
        {
            
            if (user.Clusters.FirstOrDefault(c => c.Id == cluster.Id) == null)
                return new ResultViewModel<UserDTO>(MapToDTO(user), false, "Cluster not found");

            user.Clusters.Remove(cluster);

            _context.Clusters.Remove(cluster);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "Cluster removed successfully");
        }

        public async Task<ResultViewModel<UserDTO>> RemoveAllClusters(User user)
        {
            var clusters = user.Clusters.ToList();
            user.Clusters.Clear();

            _context.Clusters.RemoveRange(clusters);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "All clusters removed successfully");
        }
        public async Task<ResultViewModel<TagDTO>> AddTag(User user, Tag tag)
        {
            if (user.Tags.Any(t => t.Id == tag.Id || t.Name == tag.Name))
                return new ResultViewModel<TagDTO>(_tagService.MapToDTO(tag), false, "Tag already exists");

            user.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TagDTO>(_tagService.MapToDTO(tag), true, "Tag added successfully");
        }

        public async Task<ResultViewModel<UserDTO>> RemoveTag(User user, Tag tag)
        {
            if (user.Tags.FirstOrDefault(t => t.Id == tag.Id) == null)
                return new ResultViewModel<UserDTO>(MapToDTO(user), false, "Tag not found");

            user.Tags.Remove(tag);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "Tag removed successfully");
        }

        public async Task<ResultViewModel<UserDTO>> RemoveAllTags(User user)
        {
            var tags = user.Tags.ToList();
            user.Tags.Clear();

            _context.Tags.RemoveRange(tags);
            await _context.SaveChangesAsync();
            return new ResultViewModel<UserDTO>(MapToDTO(user), true, "All tags removed successfully");
        }

    }
}
