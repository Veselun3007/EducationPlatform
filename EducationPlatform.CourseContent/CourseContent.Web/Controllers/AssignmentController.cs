using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController(IOperation<Assignment> crudContext) : Controller
    {

        private readonly IOperation<Assignment> _crudContext = crudContext;

        [Route("createAssignment")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAssignment([FromForm] Assignment assignment)
        {
            var newAssignment = await _crudContext.CreateAsync(assignment);
            return Ok(newAssignment);
        }

        [Route("updateAssignment/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] Assignment entity)
        {
            var updatedAssignment = await _crudContext.UpdateAsync(id, entity);

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
            await _crudContext.DeleteAsync(id);
            return Ok();
        }

        [Route("getAssignmentById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAssignment(int id)
        {
            var assignment = await _crudContext.GetByIdAsync(id);

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
            return await _crudContext.GetAllAsync();
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
    }
}
