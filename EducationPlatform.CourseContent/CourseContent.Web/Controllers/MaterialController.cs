using CourseContent.Core.Interfaces;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.CourseContent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController(IOperation<Material> crudContext) : Controller
    {
        private readonly IOperation<Material> _crudContext = crudContext;

        [Route("createMaterial")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateMaterial([FromBody] Material assignment)
        {
            var newAssignment = await _crudContext.CreateAsync(assignment);
            return Ok(newAssignment);
        }

        [Route("updateMaterial/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateMaterial(int id, Material entity)
        {
            var updatedAssignment = await _crudContext.UpdateAsync(id, entity);

            if (updatedAssignment == null)
            {
                return NotFound();
            }

            return Ok(updatedAssignment);
        }

        [Route("deleteMaterial/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            await _crudContext.DeleteAsync(id);
            return Ok();
        }

        [Route("getMaterialById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdMaterial(int id)
        {
            var assignment = await _crudContext.GetByIdAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        [Route("getMaterials")]
        [HttpGet]
        public async Task<IEnumerable<Material>> GetAllMaterial()
        {
            return await _crudContext.GetAllAsync();
        }


        [HttpDelete("removeMaterials")]
        public async Task<IActionResult> RemoveMaterials([FromBody] IEnumerable<Material> entities)
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
