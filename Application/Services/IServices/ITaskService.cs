using Application.ViewModels;
using ToDo.Application.DTO;
using ToDo.Domain.Entity;
using Task = ToDo.Domain.Entity.Task;
namespace Application.IServices
{
    internal interface ITaskService : ICRUD<Task, TaskDTO>
    {
        Task<ResultViewModel<TaskDTO>> AddTag(Task task, Tag tag);
        Task<ResultViewModel<TaskDTO>> RemoveTag(Task task, Tag tag);
        Task<ResultViewModel<TaskDTO>> AddAllTags(Task task, IEnumerable<Tag> tags);
        Task<ResultViewModel<TaskDTO>> RemoveAllTags(Task task);

    }
}
