using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    public class MaterialController : ContentController<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO>
    {
        private readonly IFileService<MaterialfileOutDTO> _fileService;
        private readonly ILinkService<MateriallinkOutDTO> _linkService;

        public MaterialController(
            IContentService<MaterialDTO, MaterialOutDTO, MaterialUpdateDTO> service,
            IFileService<MaterialfileOutDTO> fileService,
            ILinkService<MateriallinkOutDTO> linkService) : base(service) 
        {
            _fileService = fileService;
            _linkService = linkService;
        }

        [HttpGet("getFileById/{fileId}")]
        public async Task<IActionResult> GetMaterialFileById(int fileId)
        {
            var result = await _fileService.GetFileByIdAsync(fileId);
            return Ok(result);
        }

        [HttpDelete("deleteFileById/{fileId}")]
        public async Task<IActionResult> DeleteMaterialFileById(int fileId)
        {
            await _fileService.DeleteFileAsync(fileId);
            return Ok();
        }

        [HttpPost("addFile/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AddMaterialFile([FromForm] IFormFile file, int id)
        {
            var result = await _fileService.AddFileAsync(file, id);
            return Ok(result);
        }

        [HttpPost("addLink/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AddMaterialLink([FromBody] string link, int id)
        {
            var result = await _linkService.AddLinkAsync(link, id);
            return Ok(result);
        }

        [HttpDelete("deleteLinkById/{linkId}")]
        public async Task<IActionResult> DeleteMaterialLinkById(int linkId)
        {
            await _linkService.DeleteLinkAsync(linkId);
            return Ok();
        }
    }
}