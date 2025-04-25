using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/topic")]
    [ApiController]
    [Authorize]
    public class TopicController : Controller
    {
        private readonly IContentServices<TopicDTO, TopicOutDTO, TopicUpdateDTO> _contentServices;

        public TopicController(IContentServices<TopicDTO, TopicOutDTO, TopicUpdateDTO> contentServices)
        {
            _contentServices = contentServices;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTopic([FromForm] TopicDTO Topic)
        {
            var result = await _contentServices.CreateAsync(Topic);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTopic([FromForm] TopicUpdateDTO Topic)
        {
            var result = await _contentServices.UpdateAsync(Topic, Topic.Id);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            await _contentServices.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdTopic(int id)
        {
            var result = await _contentServices.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getAll/{id}")]
        public async Task<IEnumerable<TopicOutDTO>> GetAllTopic(int id)
        {
            var result = await _contentServices.GetAllByCourseAsync(id);
            return (IEnumerable<TopicOutDTO>)Ok(result);
        }

        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveTopics([FromBody] List<int> entities)
        {
            await _contentServices.RemoveRangeAsync(entities);
            return Ok();
        }
    }
}
