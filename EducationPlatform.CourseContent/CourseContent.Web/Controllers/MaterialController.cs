using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/material")]
    [ApiController]
    [Authorize]
    public class MaterialController : Controller
    {
        private readonly IContentServices<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO> _contentServices;
        private readonly IFileServices<MaterialfileOutDTO> _fileServices;
        private readonly ILinkServices<MateriallinkOutDTO> _linkServices;

        public MaterialController(IContentServices<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO> contentServices,
            IFileServices<MaterialfileOutDTO> fileServices,
            ILinkServices<MateriallinkOutDTO> linkServices)
        {
            _contentServices = contentServices;
            _fileServices = fileServices;
            _linkServices = linkServices;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMaterial([FromForm] MaterialDTO Material)
        {
            var result = await _contentServices.CreateAsync(Material);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateMaterial([FromForm] MaterialUpdateDTO Material)
        {
            var result = await _contentServices.UpdateAsync(Material, Material.Id);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            await _contentServices.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdMaterial(int id)
        {
            var result = await _contentServices.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getAll/{id}")]
        public async Task<IEnumerable<MaterialOutDTO>> GetAllMaterial(int id)
        {
            var result = await _contentServices.GetAllByCourseAsync(id);
            return (IEnumerable<MaterialOutDTO>)Ok(result);
        }

        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveMaterials([FromBody] List<int> entities)
        {
            await _contentServices.RemoveRangeAsync(entities);
            return Ok();
        }

        [HttpGet("getFileById/{fileId}")]
        public async Task<IActionResult> GetMaterialFileById(int fileId)
        {
            var result = await _fileServices.GetFileByIdAsync(fileId);
            return Ok(result);
        }

        [HttpDelete("deleteFileById/{fileId}")]
        public async Task<IActionResult> DeleteMaterialFileById(int fileId)
        {
            await _fileServices.DeleteFileAsync(fileId);
            return Ok();
        }

        [HttpPost("addFile/{id}")]
        public async Task<IActionResult> AddMaterialFile([FromForm] IFormFile file, int id)
        {
            var result = await _fileServices.AddFileAsync(file, id);
            return Ok(result);
        }

        [HttpPost("addLink/{id}")]
        public async Task<IActionResult> AddMaterialLink([FromBody] string link, int id)
        {
            var result = await _linkServices.AddLinkAsync(link, id);
            return Ok(result);
        }

        [HttpDelete("deleteLinkById/{linkId}")]
        public async Task<IActionResult> DeleteMaterialLinkById(int linkId)
        {
            await _linkServices.DeleteLinkAsync(linkId);
            return Ok();
        }
    }
}