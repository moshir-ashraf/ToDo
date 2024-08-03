using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Services;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;
        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }
        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _tagService.Get(id).Result;
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _tagService.GetAll().Result;
            return Ok(result);
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] TagDTO tag)
        {
            var result = _tagService.Create(_tagService.MapToEntity(tag)).Result;
            return CreatedAtAction(nameof(Create), result);
        }
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] TagDTO tag)
        {
            var result = _tagService.Update(_tagService.MapToEntity(tag), tag.Id).Result;
            return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _tagService.Delete(id).Result;
            return Ok(result);
        }
    }

}
