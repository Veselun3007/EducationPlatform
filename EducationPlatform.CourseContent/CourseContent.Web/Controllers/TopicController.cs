using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Web.Controllers.Base;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [ApiController]
    [Route("api/topic")]
    public class TopicController(IBaseOperation<TopicOutDTO, Error, TopicDTO> operation) : BaseController
    {
        private readonly IBaseOperation<TopicOutDTO, Error, TopicDTO> _operation = operation;

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTopic([FromForm] TopicDTO topic)
        {
            var result = await _operation.CreateAsync(topic);
            return FromResult(result);
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTopic(int id, [FromBody] TopicDTO topic)
        {
            var result = await _operation.UpdateAsync(topic, id);
            return FromResult(result);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var result = await _operation.DeleteAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdTopic(int id)
        {
            var result = await _operation.GetByIdAsync(id);
            return FromResult(result);
        }

        //[Authorize]
        [HttpGet("getAll/{id}")]
        public async Task<IEnumerable<TopicOutDTO>> GetAllTopic(int id)
        {
            return await _operation.GetAllByCourseAsync(id);
        }
    }
}
