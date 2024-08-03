using Application.ViewModels;
using ToDo.Application.DTO;
using ToDo.Domain.Entity;
using Task = ToDo.Domain.Entity.Task;
namespace Application.IServices
{
    internal interface IUserService : ICRUD<User, UserDTO>
    {
        Task<ResultViewModel<UserDTO>> GetByCredentials(string email, string password);
        Task<ResultViewModel<UserDTO>> AddCluster(User user, Cluster cluster);
        Task<ResultViewModel<UserDTO>> RemoveCluster(User user, Cluster cluster);
        Task<ResultViewModel<UserDTO>> RemoveAllClusters(User user);
        Task<ResultViewModel<TagDTO>> AddTag(User user,Tag tag);
        Task<ResultViewModel<UserDTO>> RemoveTag(User user, Tag tag);
        Task<ResultViewModel<UserDTO>> RemoveAllTags(User user);
    }
}
