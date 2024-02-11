using CourseContent.Core.DTOs;
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
        public async Task<IActionResult> CreateAssignment([FromForm] AssignmentDTO assignment)
        {
            var newAssignment = AssignmentDTO.FromAssignmentDto(assignment);
            var createdAssignment = await _crudContext.CreateAsync(newAssignment, assignment.AssignmentFiles!);
            var outAssignment = AssignmentOutDTO.FromAssignment(createdAssignment);
            return Ok(outAssignment);
        }

        [Route("updateAssignment/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] AssignmentDTO assignment)
        {
            var assignmentForPut = AssignmentDTO.FromAssignmentDto(assignment);
            var updatedAssignment = await _crudContext.UpdateAsync(id, assignmentForPut);

            if (updatedAssignment is null)
            {
                return NotFound();
            }

            var outAssignment = AssignmentOutDTO.FromAssignment(updatedAssignment);
            return Ok(outAssignment);
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

            if (assignment is null)
            {
                return NotFound();
            }
            var outAssignment = AssignmentOutDTO.FromAssignment(assignment);
            return Ok(outAssignment);
        }

        [Route("getAllAssignment/{id}")]
        [HttpGet]
        public async Task<IEnumerable<AssignmentOutDTO>> GetAllAssignment(int id)
        {
            var assignments = await _crudContext.GetAllByCourseAsync(id);
            return assignments.Select(AssignmentOutDTO.FromAssignment).ToList();
        }

        [HttpDelete("removeAssignments")]
        public async Task<IActionResult> RemoveMaterials([FromBody] IEnumerable<Assignment> entities) //rewrite for id
        {
            if (entities is null || !entities.Any())
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
            var _fileName = await _crudContext.GetFileByIdAsync(id);

            if (_fileName is null)
            {
                return NotFound();
            }

            return Ok(_fileName);
        }
    }
}
