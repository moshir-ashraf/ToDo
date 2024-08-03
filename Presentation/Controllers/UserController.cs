using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTO;
using ToDo.Application.Services;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ClusterService _clusterService;
        private readonly TagService _tagService;
        public UserController(UserService userService, ClusterService clusterService, TagService tagService)
        {
            _userService = userService;
            _clusterService = clusterService;
            _tagService = tagService;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _userService.Get(id).Result;
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll().Result;
            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserDTO user)
        {
            var result = _userService.Create(_userService.MapToEntity(user)).Result;
            return CreatedAtAction(nameof(Create), result);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id,[FromBody] UserDTO user)
        {
            var result = _userService.Update(_userService.MapToEntity(user), user.Id).Result;
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id).Result;
            return Ok(result);
        }

        [HttpGet("GetByCredentials")]
        public IActionResult GetByCredentials(string email, string password)
        {
            var result = _userService.GetByCredentials(email, password).Result;
            return Ok(result);
        }

        [HttpPut("AddCluster")]
        public IActionResult AddCluster(int userId, [FromBody] ClusterDTO cluster)
        {
            var user = _userService.Get(userId).Result.Data;
            var result = _userService.AddCluster(_userService.MapToEntity(user), _clusterService.MapToEntity(cluster)).Result;
            return Ok(result);
        }

        [HttpPut("AddTag")]
        public IActionResult AddTag(int userId, [FromBody] TagDTO tag)
        {
            var user = _userService.Get(userId).Result.Data;
            var result = _userService.AddTag(_userService.MapToEntity(user), _tagService.MapToEntity(tag)).Result;
            return Ok(result);
        }

        [HttpPut("RemoveCluster")]
        public IActionResult RemoveCluster(int userId, [FromBody] ClusterDTO cluster)
        {
            var user = _userService.Get(userId).Result.Data;
            var result = _userService.RemoveCluster(_userService.MapToEntity(user), _clusterService.MapToEntity(cluster)).Result;
            return Ok(result);
        }

        [HttpPut("RemoveTag")]
        public IActionResult RemoveTag(int userId, [FromBody] TagDTO tag)
        {
            var user = _userService.Get(userId).Result.Data;
            var result = _userService.RemoveTag(_userService.MapToEntity(user), _tagService.MapToEntity(tag)).Result;
            return Ok(result);
        }

        [HttpPut("RemoveAllClusters")]
        public IActionResult RemoveAllClusters([FromBody] UserDTO user)
        {
            var result = _userService.RemoveAllClusters(_userService.MapToEntity(user)).Result;
            return Ok(result);
        }

        [HttpPut("RemoveAllTags")]
        public IActionResult RemoveAllTags([FromBody] UserDTO user)
        {
            var result = _userService.RemoveAllTags(_userService.MapToEntity(user)).Result;
            return Ok(result);
        }

    }
}
