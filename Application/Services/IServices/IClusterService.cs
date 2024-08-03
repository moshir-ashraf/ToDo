using Application.ViewModels;
using ToDo.Application.DTO;
using ToDo.Domain.Entity;
using Task = ToDo.Domain.Entity.Task;

namespace Application.IServices
{
    internal interface IClusterService : ICRUD<Cluster,ClusterDTO>
    {
        Task<ResultViewModel<ClusterDTO>> AddTask(Cluster cluster, Task task);
        Task<ResultViewModel<ClusterDTO>> RemoveTask(Cluster cluster, Task task);
        Task<ResultViewModel<ClusterDTO>> AddAllTasks(Cluster cluster, IEnumerable<Task> tasks);
        Task<ResultViewModel<ClusterDTO>> RemoveAllTasks(Cluster cluster);
    }
}
