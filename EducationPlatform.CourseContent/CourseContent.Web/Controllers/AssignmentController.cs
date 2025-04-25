using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    [Authorize]
    public class AssignmentController : Controller
    {
        private readonly IContentServices<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO> _contentServices;
        private readonly IFileServices<AssignmentfileOutDTO> _fileServices;
        private readonly ILinkServices<AssignmentlinkOutDTO> _linkServices;

        public AssignmentController(IContentServices<AssignmentDTO, AssignmentOutDTO, AssignmentUpdateDTO> contentServices,
            IFileServices<AssignmentfileOutDTO> fileServices,
            ILinkServices<AssignmentlinkOutDTO> linkServices)
        {
            _contentServices = contentServices;
            _fileServices = fileServices;
            _linkServices = linkServices;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromForm] AssignmentDTO assignment)
        {
            var result = await _contentServices.CreateAsync(assignment);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAssignment([FromForm] AssignmentUpdateDTO assignment)
        {
            var result = await _contentServices.UpdateAsync(assignment, assignment.Id);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            await _contentServices.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdAssignment(int id)
        {
            var result = await _contentServices.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getAll/{id}")]
        public async Task<IActionResult> GetAllAssignment(int id)
        {
            var result = await _contentServices.GetAllByCourseAsync(id);
            return Ok(result);
        }

        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveAssignments([FromBody] List<int> entities)
        {
            await _contentServices.RemoveRangeAsync(entities);
            return Ok();
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
        public async Task<IActionResult> AddAssignmentFile([FromForm] IFormFile file, int id)
        {
            var result = await _fileServices.AddFileAsync(file, id);
            return Ok(result);
        }

        [HttpPost("addLink/{id}")]
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
