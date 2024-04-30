﻿using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Models.ErrorModels;
using CourseContent.Domain.Entities;
using CourseContent.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.CourseContent.Controllers
{
    [Route("api/material")]
    [ApiController]
    public class MaterialController(IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO, MaterialUpdateDTO, Materiallink> operation) : BaseController
    {
        private readonly IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO, MaterialUpdateDTO, Materiallink> _operation = operation;
        
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateMaterial([FromForm] MaterialDTO material)
        {
            var result = await _operation.CreateAsync(material);
            return FromResult(result);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMaterial([FromForm] MaterialUpdateDTO material)
        {
            var result = await _operation.UpdateAsync(material, material.Id);
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
            var result = await _operation.RemoveRangeAsync(entities);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet("getFileById/{fileId}")]
        public async Task<IActionResult> GetMaterialFileById(int fileId)
        {
            var result = await _operation.GetFileByIdAsync(fileId);
            return FromResult(result);
        }

        [Authorize]
        [HttpDelete("deleteFileById/{fileId}")]
        public async Task<IActionResult> DeleteMaterialFileById(int fileId)
        {
            var result = await _operation.DeleteFileAsync(fileId);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost("addFile/{id}")]
        public async Task<IActionResult> AddMaterialFile([FromForm] IFormFile file, int id)
        {
            var result = await _operation.AddFileAsync(file, id);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost("addLink/{id}")]
        public async Task<IActionResult> AddMaterialLink([FromBody] string link, int id)
        {
            var result = await _operation.AddLinkAsync(link, id);
            return FromResult(result);
        }

        [Authorize]
        [HttpDelete("deleteLinkById/{fileId}")]
        public async Task<IActionResult> DeleteMaterialLinkById(int fileId)
        {
            var result = await _operation.DeleteLinkAsync(fileId);
            return FromResult(result);
        }
    }
}
