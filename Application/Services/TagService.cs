using ToDo.Infrastructure.Data;
using Application.IServices;
using ToDo.Application.DTO;
using Application.ViewModels;
using ToDo.Domain.Entity;
using Microsoft.EntityFrameworkCore;
namespace ToDo.Application.Services
{
    public class TagService : ICRUD<Tag, TagDTO>, IDTOMapper<Tag, TagDTO>
    {
        private readonly AppDbContext _context;
        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public  TagDTO MapToDTO(Tag data)
        {
            return new TagDTO
            {
                Id = data.Id,
                Name = data.Name,
                Priority = data.Priority ?? string.Empty,
                UserId = data.UserId,
                UserName = data.User.Name ?? string.Empty
            };
        }

        public Tag MapToEntity(TagDTO data)
        {
            return new Tag
            {
                Id = data.Id,
                Name = data.Name,
                Priority = data.Priority ?? string.Empty,
                UserId = data.UserId,
            };
        }

        public async Task<ResultViewModel<TagDTO>> Create(Tag data)
        {
            if (await _context.Tags.FirstOrDefaultAsync(x => x.Id == data.Id) != null)
                return new ResultViewModel<TagDTO>(null, false, "Tag already exists");

            await _context.Tags.AddAsync(data);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TagDTO>(MapToDTO(data), true, "Tag created successfully");
        }

        public async Task<ResultViewModel<TagDTO>> Delete(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag == null)
                return new ResultViewModel<TagDTO>(null, false, "Tag not found");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return new ResultViewModel<TagDTO>(MapToDTO(tag), true, "Tag deleted successfully");
        }

        public async Task<ResultViewModel<TagDTO>> Get(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag == null)
                return new ResultViewModel<TagDTO>(null, false, "Tag not found");

            return new ResultViewModel<TagDTO>(MapToDTO(tag), true, "Tag retrieved successfully");
        }

        public async Task<ResultViewModel<IEnumerable<TagDTO>>> GetAll()
        {
            var tags = await _context.Tags.ToListAsync();

            if (tags.Count == 0)
                return new ResultViewModel<IEnumerable<TagDTO>>(new List<TagDTO>(), false, "No tags found");

            var tagsDTO = tags.Select(x => MapToDTO(x));
            return new ResultViewModel<IEnumerable<TagDTO>>(tagsDTO, true, "Tags retrieved successfully");
        }

        public async Task<ResultViewModel<TagDTO>> Update(Tag data, int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag == null)
                return new ResultViewModel<TagDTO>(null, false, "Tag not found");

            tag.Name = data.Name;
            tag.Priority = data.Priority;

            await _context.SaveChangesAsync();
            return new ResultViewModel<TagDTO>(MapToDTO(tag), true, "Tag updated successfully");
        }
    }
}
