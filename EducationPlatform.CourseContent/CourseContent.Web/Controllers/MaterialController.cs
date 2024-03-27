using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Web.Controllers.Base;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.CourseContent.Controllers
{
    [Route("api/material")]
    [ApiController]
    public class MaterialController(IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO> operation) : BaseController
    {
        private readonly IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO> _operation = operation;
        
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateMaterial([FromForm] MaterialDTO material)
        {
            var result = await _operation.CreateAsync(material);
            return FromResult(result);
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, [FromBody] MaterialDTO material)
        {
            var result = await _operation.UpdateAsync(material, id);
            return FromResult(result);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var result = await _operation.DeleteAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdMaterial(int id)
        {
            var result = await _operation.GetByIdAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("getAll/{id}")]
        public async Task<IEnumerable<MaterialOutDTO>> GetAllMaterial(int id)
        {
            return await _operation.GetAllByCourseAsync(id);
        }

        [Authorize]
        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveMaterials([FromBody] List<int> entities)
        {
            if (entities.Count == 0)
            {
                return BadRequest("No entities provided for removal.");
            }

            var result = await _operation.RemoveRangeAsync(entities);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("getFileById/{id}")]
        public async Task<IActionResult> GetMaterialFileById(int id)
        {
            var result = await _operation.GetFileByIdAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpDelete("deleteFileById/{id}")]
        public async Task<IActionResult> DeleteMaterialFileById(int id)
        {
            var result = await _operation.DeleteFileAsync(id);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost("addFileById/{id}")]
        public async Task<IActionResult> AddMaterialFileById([FromForm] IFormFile file, int id)
        {
            var result = await _operation.AddFileAsync(file, id);
            return FromResult(result);
        }
    }
}
