using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Services;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly TagService _tagService;
        public TaskController(TaskService taskService, TagService tagService)
        {
            _taskService = taskService;
            _tagService = tagService;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _taskService.Get(id).Result;
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _taskService.GetAll().Result;
            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] TaskDTO task)
        {
            var result = _taskService.Create(_taskService.MapToEntity(task)).Result;
            return CreatedAtAction(nameof(Create), result);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id,[FromBody] TaskDTO task)
        {
            var result = _taskService.Update(_taskService.MapToEntity(task), task.Id).Result;
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _taskService.Delete(id).Result;
            return Ok(result);
        }

        [HttpPut("AddTag")]
        public IActionResult AddTag(int tagId, [FromBody] TaskDTO task)
        {
            var tag = _tagService.Get(tagId).Result.Data;
            var result = _taskService.AddTag(_taskService.MapToEntity(task), _tagService.MapToEntity(tag)).Result;
            return Ok(result);
        }

        [HttpPut("RemoveTag")]
        public IActionResult RemoveTag(int tagId, [FromBody] TaskDTO task)
        {
            var tag = _tagService.Get(tagId).Result.Data;
            var result = _taskService.RemoveTag(_taskService.MapToEntity(task), _tagService.MapToEntity(tag)).Result;
            return Ok(result);
        }

        [HttpPut("AddAllTags")]
        public IActionResult AddAllTags(int taskId, [FromBody] List<TagDTO> tags)
        {
            var task = _taskService.Get(taskId).Result.Data;
            var mappedTags = tags.Select(x => _tagService.MapToEntity(x)).ToList();
            var result = _taskService.AddAllTags(_taskService.MapToEntity(task), mappedTags).Result;
            return Ok(result);
        }

        [HttpPut("RemoveAllTags/{taskId}")]
        public IActionResult RemoveAllTags(int taskId)
        {
            var task = _taskService.Get(taskId).Result.Data;
            var result = _taskService.RemoveAllTags(_taskService.MapToEntity(task)).Result;
            return Ok(result);
        }
    }
}
