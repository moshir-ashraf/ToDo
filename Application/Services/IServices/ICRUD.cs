using Application.ViewModels;
namespace Application.IServices
{
    internal interface ICRUD<Entity,EntityDTO>
    {
        Task<ResultViewModel<EntityDTO>> Create(Entity data);
        Task<ResultViewModel<EntityDTO>> Update(Entity data, int id);
        Task<ResultViewModel<EntityDTO>> Delete(int id);
        Task<ResultViewModel<EntityDTO>> Get(int id);
        Task<ResultViewModel<IEnumerable<EntityDTO>>> GetAll();
    }
}
