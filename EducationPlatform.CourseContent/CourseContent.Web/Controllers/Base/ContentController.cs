using CourseContent.Core.DTO.Requests;
using CourseContent.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers.Base
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ContentController<IEntity, OEntity, UEntity> : Controller
        where IEntity : class where OEntity : class where UEntity : BaseUpdateDTO
    {
        private readonly IContentService<IEntity, OEntity, UEntity> _service;

        protected ContentController(IContentService<IEntity, OEntity, UEntity> service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTopic([FromForm] IEntity entity)
        {
            var result = await _service.CreateAsync(entity);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTopic([FromForm] UEntity entity)
        {
            var result = await _service.UpdateAsync(entity, entity.Id);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdTopic(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getAll/{id}")]
        public async Task<IActionResult> GetAllTopic(int id)
        {
            var result = await _service.GetAllByCourseAsync(id);
            return Ok(result);
        }

        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveTopics([FromBody] List<int> entities)
        {
            await _service.RemoveRangeAsync(entities);
            return Ok();
        }
    }
}
