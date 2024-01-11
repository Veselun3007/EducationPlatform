using CourseContent.Core.Services;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController(OperationsContext<Assignment> crudContext) : Controller
    {

        private readonly OperationsContext<Assignment> _crudContext = crudContext;

        [Route("createAssignment")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAssignment([FromForm] Assignment assignment)
        {
            var newAssignment = await _crudContext.Create(assignment);
            return Ok(newAssignment);
        }

        [Route("updateAssignment/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] Assignment entity)
        {
            var updatedAssignment = await _crudContext.Update(id, entity);

            if (updatedAssignment == null)
            {
                return NotFound();
            }

            return Ok(updatedAssignment);
        }

        [Route("deleteAssignment/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssignment(int id)
        {          
            await _crudContext.Delete(id);
            return Ok();
        }

        [Route("getAssignmentById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAssignment(int id)
        {
            var assignment = await _crudContext.GetById(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        [Route("getAllAssignment")]
        [HttpGet]
        public async Task<IEnumerable<Assignment>> GetAllAssignment()
        {
            return await _crudContext.GetAll();
        }

        [HttpDelete("removeAssignments")]
        public async Task<IActionResult> RemoveMaterials([FromBody] IEnumerable<Assignment> entities)
        {
            if (entities == null || !entities.Any())
            {
                return BadRequest("No entities provided for removal.");
            }

            await _crudContext.RemoveRangeAsync(entities);
            return Ok("Entities successfully removed.");
        }

        [Route("getAssignmentFileById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAssignmentFileById(int id)
        {
            var _fileName = await _crudContext.GetFileById(id);

            if (_fileName == null)
            {
                return NotFound();
            }

            return Ok(_fileName);
        }
    }
}
