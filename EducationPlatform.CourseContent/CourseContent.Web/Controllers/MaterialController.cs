using CourseContent.Core.DTOs;
using CourseContent.Core.Services;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.CourseContent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController(OperationsContext<Material> crudContext) : Controller
    {
        private readonly OperationsContext<Material> _crudContext = crudContext;

        [Route("createMaterial")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateMaterial([FromBody] MaterialDTO materialDto)
        {
            var material = MaterialDTO.FromMaterialDto(materialDto);
            var createdMaterial = await _crudContext.CreateAsync(material, materialDto.MaterialFiles);
            var outMaterial = MaterialOutDTO.FromMaterial(createdMaterial);
            return Ok(outMaterial);
        }

        [Route("updateMaterial/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateMaterial(int id, Material entity)
        {
            var updatedAssignment = await _crudContext.UpdateAsync(id, entity);

            if (updatedAssignment is null)
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

            if (assignment is null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        [Route("getMaterials/{id}")]
        [HttpGet]
        public IEnumerable<MaterialOutDTO> GetAllMaterials(int id)
        {
            return _crudContext.GetByCourse(id).Select(MaterialOutDTO.FromMaterial);
        }


        [HttpDelete("removeMaterials")]
        public async Task<IActionResult> RemoveMaterials([FromBody] IEnumerable<Material> entities)
        {
            if (entities is null || !entities.Any())
            {
                return BadRequest("No entities provided for removal.");
            }

            await _crudContext.RemoveRangeAsync(entities);
            return Ok("Entities successfully removed.");
        }

        [Route("getMaterialFileById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetMaterialFileById(int id)
        {
            var _fileName = await _crudContext.GetFileById(id);

            if (_fileName is null)
            {
                return NotFound();
            }

            return Ok(_fileName);
        }
    }
}
