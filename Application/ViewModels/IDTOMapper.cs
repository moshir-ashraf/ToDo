namespace Application.ViewModels
{
    internal interface IDTOMapper<Entity,EntityDTO>
    {
         EntityDTO MapToDTO(Entity data);
         Entity MapToEntity(EntityDTO data);
    }
}
