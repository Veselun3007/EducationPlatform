using CourseContent.Core.DTO.Requests.AssignmentDTO;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Models.ErrorModels;
using CourseContent.Domain.Entities;
using CourseContent.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    [Authorize]
    public class AssignmentController(IOperation<AssignmentOutDTO, Error, AssignmentDTO,
        AssignmentfileOutDTO, AssignmentUpdateDTO, Assignmentlink> operation) : BaseController
    {
        private readonly IOperation<AssignmentOutDTO, Error, AssignmentDTO,
            AssignmentfileOutDTO, AssignmentUpdateDTO, Assignmentlink> _operation = operation;

        [HttpPost("create")]
        public async Task<IActionResult> CreateAssignment([FromForm] AssignmentDTO assignment)
        {
            var result = await _operation.CreateAsync(assignment);
            return FromResult(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAssignment([FromForm] AssignmentUpdateDTO assignment)
        {
            var result = await _operation.UpdateAsync(assignment, assignment.Id);
            return FromResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var result = await _operation.DeleteAsync(id);
            return FromResult(result);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetByIdAssignment(int id)
        {
            var result = await _operation.GetByIdAsync(id);
            return FromResult(result);
        }

        [HttpGet("getAll/{id}")]
        public async Task<IEnumerable<AssignmentOutDTO>> GetAllAssignment(int id)
        {
            return await _operation.GetAllByCourseAsync(id);
        }

        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveAssignments([FromBody] List<int> entities)
        {
            var result = await _operation.RemoveRangeAsync(entities);
            return FromResult(result);
        }

        [HttpGet("getFileById/{fileId}")]
        public async Task<IActionResult> GetAssignmentFileById(int fileId)
        {
            var result = await _operation.GetFileByIdAsync(fileId);
            return FromResult(result);
        }

        [HttpDelete("deleteFileById/{fileId}")]
        public async Task<IActionResult> DeleteAssignmentFileById(int fileId)
        {
            var result = await _operation.DeleteFileAsync(fileId);
            return FromResult(result);
        }

        [HttpPost("addFile/{id}")]
        public async Task<IActionResult> AddAssignmentFile([FromForm] IFormFile file, int id)
        {
            var result = await _operation.AddFileAsync(file, id);
            return FromResult(result);
        }

        [HttpPost("addLink/{id}")]
        public async Task<IActionResult> AddAssignmentLink([FromBody] string link, int id)
        {
            var result = await _operation.AddLinkAsync(link, id);
            return FromResult(result);
        }

        [HttpDelete("deleteLinkById/{linkId}")]
        public async Task<IActionResult> DeleteAssignmentLinkById(int linkId)
        {
            var result = await _operation.DeleteLinkAsync(linkId);
            return FromResult(result);
        }
    }
}
