﻿using CourseContent.Core.DTO.Requests.AssignmentDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Web.Controllers.Base;
using Identity.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseContent.Web.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    public class AssignmentController(IOperation<AssignmentOutDTO, Error, AssignmentDTO, AssignmentfileOutDTO> operation) : BaseController
    {
        private readonly IOperation<AssignmentOutDTO, Error, AssignmentDTO, AssignmentfileOutDTO> _operation = operation;

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> CreateAssignment([FromForm] AssignmentDTO assignment)
        {
            var result = await _operation.CreateAsync(assignment);
            return FromResult(result);
        }

        [HttpPut]
        [Authorize]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] AssignmentDTO assignment)
        {
            var result = await _operation.UpdateAsync(assignment, id);
            return FromResult(result);
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var result = await _operation.DeleteAsync(id);
            return FromResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetByIdAssignment(int id)
        {
            var result = await _operation.GetByIdAsync(id);
            return FromResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getAll/{id}")]
        public async Task<IEnumerable<AssignmentOutDTO>> GetAllAssignment(int id)
        {
            return await _operation.GetAllByCourseAsync(id);
        }

        [HttpDelete("removeList")]
        public async Task<IActionResult> RemoveAssignments([FromBody] List<int> entities)
        {
            if (entities.Count == 0)
            {
                return BadRequest("No entities provided for removal.");
            }

            var result = await _operation.RemoveRangeAsync(entities);
            return FromResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("getFileById/{id}")]
        public async Task<IActionResult> GetAssignmentFileById(int id)
        {
            var result = await _operation.GetFileByIdAsync(id);
            return FromResult(result);
        }

        [HttpDelete]
        [Authorize]
        [Route("deleteFileById/{id}")]
        public async Task<IActionResult> DeleteAssignmentFileById(int id)
        {
            var result = await _operation.DeleteFileAsync(id);
            return FromResult(result);
        }

        [HttpPost]
        [Authorize]
        [Route("addFileById/{id}")]
        public async Task<IActionResult> AddAssignmentFileById([FromForm] IFormFile file, int id)
        {
            var result = await _operation.AddFileAsync(file, id);
            return FromResult(result);
        }
    }
}
