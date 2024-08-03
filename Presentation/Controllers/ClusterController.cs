using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Services;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClusterController : ControllerBase
    {
        private readonly ClusterService _clusterService;
        private readonly TaskService _taskService;
        public ClusterController(ClusterService clusterService, TaskService taskService)
        {
            _clusterService = clusterService;
            _taskService = taskService;
        }
        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _clusterService.Get(id).Result;
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _clusterService.GetAll().Result;
            return Ok(result);
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] ClusterDTO cluster)
        {
            var result = _clusterService.Create(_clusterService.MapToEntity(cluster)).Result;
            return CreatedAtAction(nameof(Create),result);
        }
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id,[FromBody] ClusterDTO cluster)
        {
            var result = _clusterService.Update(_clusterService.MapToEntity(cluster), cluster.Id).Result;
            return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _clusterService.Delete(id).Result;
            return Ok(result);
        }

        [HttpPut("AddTask")]
        public IActionResult AddTask(int clusterId, [FromBody] TaskDTO task)
        {
            var cluster = _clusterService.Get(clusterId).Result.Data;
            var result = _clusterService.AddTask(_clusterService.MapToEntity(cluster),_taskService.MapToEntity(task)).Result;
            return Ok(result);
        }

        [HttpPut("RemoveTask")]
        public IActionResult RemoveTask(int clusterId, [FromBody] TaskDTO task)
        {
            var cluster = _clusterService.Get(clusterId).Result.Data;
            var result = _clusterService.RemoveTask(_clusterService.MapToEntity(cluster),_taskService.MapToEntity(task)).Result;
            return Ok(result);
        }

        [HttpPut("AddAllTasks")]
        public IActionResult AddAllTasks(int clusterId, [FromBody] List<TaskDTO> tasks)
        {
            var cluster = _clusterService.Get(clusterId).Result.Data;
            var result = _clusterService.AddAllTasks(_clusterService.MapToEntity(cluster),tasks.Select(x => _taskService.MapToEntity(x)).ToList()).Result;
            return Ok(result);
        }

        [HttpPut("RemoveAllTasks")]
        public IActionResult RemoveAllTasks([FromBody] ClusterDTO cluster)
        {
            var result = _clusterService.RemoveAllTasks(_clusterService.MapToEntity(cluster)).Result;
            return Ok(result);
        }
    }
}
