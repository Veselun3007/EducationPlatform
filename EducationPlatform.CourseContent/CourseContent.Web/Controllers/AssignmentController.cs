using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    public class AssignmentController : ContentController<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO>
    {      
        private readonly IFileService<AssignmentfileOutDTO> _fileServices;
        private readonly ILinkService<AssignmentlinkOutDTO> _linkServices;

        public AssignmentController(
            IContentService<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO> service,
            IFileService<AssignmentfileOutDTO> fileServices,
            ILinkService<AssignmentlinkOutDTO> linkServices) : base(service)
        {
            _fileServices = fileServices;
            _linkServices = linkServices;
        }
    
        [HttpGet("getFileById/{fileId}")]
        public async Task<IActionResult> GetAssignmentFileById(int fileId)
        {
            var result = await _fileServices.GetFileByIdAsync(fileId);
            return Ok(result);
        }

        [HttpDelete("deleteFileById/{fileId}")]
        public async Task<IActionResult> DeleteAssignmentFileById(int fileId)
        {
            await _fileServices.DeleteFileAsync(fileId);
            return Ok();
        }

        [HttpPost("addFile/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AddAssignmentFile([FromForm] IFormFile file, int id)
        {
            var result = await _fileServices.AddFileAsync(file, id);
            return Ok(result);
        }

        [HttpPost("addLink/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> AddAssignmentLink([FromBody] string link, int id)
        {
            var result = await _linkServices.AddLinkAsync(link, id);
            return Ok(result);
        }

        [HttpDelete("deleteLinkById/{linkId}")]
        public async Task<IActionResult> DeleteAssignmentLinkById(int linkId)
        {
            await _linkServices.DeleteLinkAsync(linkId);
            return Ok();
        }
    }
}
