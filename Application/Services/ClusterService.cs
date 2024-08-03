using ToDo.Domain.Entity;
using Application.IServices;
using Application.ViewModels;
using ToDo.Application.DTO;
using ToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Task = ToDo.Domain.Entity.Task;

namespace ToDo.Application.Services
{
    public class ClusterService : IClusterService, IDTOMapper<Cluster, ClusterDTO>
    {
        private readonly AppDbContext _context;
        private readonly TaskService _taskService;
        public ClusterService(AppDbContext context, TaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        public ClusterDTO MapToDTO(Cluster data)
        {
            var tasks = data.Tasks
                    .Select(x => _taskService.MapToDTO(x))
                    .ToList();
            
            return new ClusterDTO
            {
                Id = data.Id,
                Name = data.Name,
                Status = data.Status,
                Description = data.Description ?? string.Empty,
                Tasks = tasks,
                UserId = data.UserId,   
                UserName = data.User.Name ?? string.Empty
            };
        }

        public Cluster MapToEntity(ClusterDTO data)
        {
            return new Cluster
            {
                Id = data.Id,
                Name = data.Name,
                Status = data.Status,
                Description = data.Description ?? string.Empty,
                UserId = data.UserId,
                Tasks = data.Tasks.Select(x => _taskService.MapToEntity(x)).ToList()
            };
        }

        public async Task<ResultViewModel<ClusterDTO>> Create(Cluster data)
        {
            if (await _context.Clusters.FirstOrDefaultAsync(x => x.Id == data.Id) != null)
                return new ResultViewModel<ClusterDTO>(null, false, "Cluster already exists");

            await _context.Clusters.AddAsync(data);
            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(data), true, "Cluster created successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> Delete(int id)
        {
            var cluster = await _context.Clusters.FirstOrDefaultAsync(x => x.Id == id);
            if (cluster == null)
                return new ResultViewModel<ClusterDTO>(null, false, "Cluster not found");

            _context.Clusters.Remove(cluster);
            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "Cluster deleted successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> Get(int id)
        {
            var cluster = await _context.Clusters.FirstOrDefaultAsync(x => x.Id == id);
            if (cluster == null)
                return new ResultViewModel<ClusterDTO>(null, false, "Cluster not found");

            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "Cluster retrieved successfully");
        }
        public async Task<ResultViewModel<IEnumerable<ClusterDTO>>> GetAll()
        {
            var clusters = await _context.Clusters.ToListAsync();
            if (clusters.Count == 0)
                return new ResultViewModel<IEnumerable<ClusterDTO>>(null, false, "No clusters found");

            var clusterDTOs = clusters.Select(x => MapToDTO(x));
            return new ResultViewModel<IEnumerable<ClusterDTO>>(clusterDTOs, true, "Clusters retrieved successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> Update(Cluster data, int id)
        {
            var cluster = await _context.Clusters.FirstOrDefaultAsync(x => x.Id == id);
            if (cluster == null)
                return new ResultViewModel<ClusterDTO>(null, false, "Cluster not found");

            cluster.Name = data.Name;
            cluster.Description = data.Description;
            cluster.Status = data.Status;

            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "Cluster updated successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> AddTask(Cluster cluster, Task task)
        {
            if (cluster.Tasks.Any(x => x.Id == task.Id))
                return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), false, "Task already added to cluster");

            cluster.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "Tag added to task successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> RemoveTask(Cluster cluster, Task task)
        {
            if (!cluster.Tasks.Any(x => x.Id == task.Id))
                return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), false, "Task not found in cluster");

            cluster.Tasks.Remove(task);

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "Task removed from cluster successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> AddAllTasks(Cluster cluster, IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (!cluster.Tasks.Any(x => x.Id == task.Id))
                    cluster.Tasks.Add(task);
            }

            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "Tasks added to task successfully");
        }

        public async Task<ResultViewModel<ClusterDTO>> RemoveAllTasks(Cluster cluster)
        {
            var tasks = cluster.Tasks.ToList();
            cluster.Tasks.Clear();
            _context.Tasks.RemoveRange(tasks);
            await _context.SaveChangesAsync();
            return new ResultViewModel<ClusterDTO>(MapToDTO(cluster), true, "All Tasks removed from task successfully");
        }
    }
}
