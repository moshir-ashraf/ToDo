using ToDo.Infrastructure.Data;
using Application.IServices;
using Application.ViewModels;
using ToDo.Application.DTO;
using Task = ToDo.Domain.Entity.Task;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entity;
namespace ToDo.Application.Services
{
    public class TaskService : IDTOMapper<Task, TaskDTO>, ITaskService
    {
        private readonly AppDbContext _context;
        private readonly TagService _tagService;
        public TaskService(AppDbContext context, TagService tagService)
        {
            _context = context;
            _tagService = tagService;
        }
        public  TaskDTO MapToDTO(Task data)
        {
            var tags = data.TaskTags
                    .Select(x => x.Tag)
                    .Select(t => _tagService.MapToDTO(t))
                    .ToList();

            return new TaskDTO
            {
                Id = data.Id,
                Name = data.Name,
                IsComplete = data.IsComplete,
                Description = data.Description ?? string.Empty,
                Tags = tags,
                UserId = data.UserId,
                UserName = data.User.Name ?? string.Empty,
                ClusterId = data.ClusterId,
            };
        }

        public Task MapToEntity(TaskDTO data)
        {
            return new Task
            {
                Id = data.Id,
                Name = data.Name,
                IsComplete = data.IsComplete,
                Description = data.Description ?? string.Empty,
                UserId = data.UserId,
                ClusterId = data.ClusterId,
                TaskTags = data.Tags.Select(x => _tagService.MapToEntity(x)).Select(t => new TaskTag { TaskId = data.Id, TagId = t.Id }).ToList()
            };
        }

        public async Task<ResultViewModel<TaskDTO>> Create(Task data)
        {
            if (await _context.Tasks.FirstOrDefaultAsync(x => x.Id == data.Id) != null)
                return new ResultViewModel<TaskDTO>(null, false, "Task already exists");

            await _context.Tasks.AddAsync(data);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(data), true, "Task created successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> Delete(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
                return new ResultViewModel<TaskDTO>(null, false, "Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "Task deleted successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> Get(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
                return new ResultViewModel<TaskDTO>(null, false, "Task not found");

            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "Task retrieved successfully");
        }

        public async Task<ResultViewModel<IEnumerable<TaskDTO>>> GetAll()
        {
            var tasks = await _context.Tasks.ToListAsync();
            if (tasks.Count == 0)
                return new ResultViewModel<IEnumerable<TaskDTO>>(null, false, "No tasks found");

            return new ResultViewModel<IEnumerable<TaskDTO>>(tasks.Select(x => MapToDTO(x)), true, "Tasks retrieved successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> Update(Task data, int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
                return new ResultViewModel<TaskDTO>(null, false, "Task not found");

            task.Name = data.Name;
            task.Description = data.Description;
            task.IsComplete = data.IsComplete;

            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "Task updated successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> AddTag(Task task, Tag tag)
        {
            if(task.TaskTags.Any(x => x.TagId == tag.Id))
                return new ResultViewModel<TaskDTO>(MapToDTO(task), false, "Tag already added to task");

            var taskTag = new TaskTag { TagId = tag.Id, TaskId = task.Id, Tag=tag};
            task.TaskTags.Add(taskTag);
            tag.TagTasks.Add(taskTag);

            await _context.TasksTags.AddAsync(taskTag);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "Tag added to task successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> RemoveTag(Task task, Tag tag)
        {
            var taskTag = task.TaskTags.FirstOrDefault(x => x.TagId == tag.Id);
            if (taskTag == null)
                return new ResultViewModel<TaskDTO>(MapToDTO(task), false, "Tag not found in task");

            task.TaskTags.Remove(taskTag);
            tag.TagTasks.Remove(taskTag);

            _context.TasksTags.Remove(taskTag);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "Tag removed from task successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> AddAllTags(Task task, IEnumerable<Tag> tags)
        {
            var tasktags = new List<TaskTag>();
            foreach (var tag in tags)
            {
                if (!task.TaskTags.Any(x => x.TagId == tag.Id))
                {
                    var taskTag = new TaskTag { TagId = tag.Id, TaskId = task.Id, Tag = tag};
                    tasktags.Add(taskTag);
                    task.TaskTags.Add(taskTag);
                    tag.TagTasks.Add(taskTag);
                }
            }

            await _context.TasksTags.AddRangeAsync(tasktags);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "Tags added to task successfully");
        }

        public async Task<ResultViewModel<TaskDTO>> RemoveAllTags(Task task)
        {
            var tasktags = task.TaskTags.ToList();
            task.TaskTags.Clear();
            _context.TasksTags.RemoveRange(tasktags);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TaskDTO>(MapToDTO(task), true, "All tags removed from task successfully");
        }

    }
}
