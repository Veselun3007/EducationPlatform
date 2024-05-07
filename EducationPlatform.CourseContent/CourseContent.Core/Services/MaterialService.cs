using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Requests.UpdateDTO;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Helpers;
using CourseContent.Core.Interfaces;
using CourseContent.Core.Models.ErrorModels;
using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.Services
{
    public class MaterialService(IUnitOfWork unitOfWork, FileHelper fileHelper) : 
        IOperation<MaterialOutDTO, Error, MaterialDTO, MaterialfileOutDTO, MaterialUpdateDTO, Materiallink>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FileHelper _fileHelper = fileHelper;

        public async Task<Result<MaterialOutDTO, Error>> CreateAsync(MaterialDTO entity)
        {
            var material = MaterialDTO.FromMaterialDto(entity);
            await _unitOfWork.MaterialRepository.AddAsync(material);
            await _unitOfWork.CompleteAsync();

            if (entity.MaterialFiles is not null)
            {
                await AddFilesAsync(material, entity.MaterialFiles);
            }
            if (entity.MaterialLinks is not null)
            {
                await AddLinksAsync(material, entity.MaterialLinks);
            }
            return Result.Success<MaterialOutDTO, Error>(MaterialOutDTO.FromMaterial(material));
        }

        private async Task AddFilesAsync(Material entity, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileLink = await _fileHelper.AddFileAsync(file);
                _unitOfWork.MaterialRepository.AddFile(entity, fileLink);
            }
            await _unitOfWork.CompleteAsync();
        }


        private async Task AddLinksAsync(Material entity, List<string> links)
        {
            foreach (var link in links)
            {
                _unitOfWork.MaterialRepository.AddLink(entity, link);
            }
            await _unitOfWork.CompleteAsync();
        }


        public async Task<Result<MaterialOutDTO, Error>> UpdateAsync(MaterialUpdateDTO entity, int id)
        {
            try
            {
                var material = MaterialUpdateDTO.FromMaterialUpdateDto(entity);
                material.IsEdited = true;
                material.EditedTime = DateTime.UtcNow;
                await _unitOfWork.MaterialRepository.UpdateAsync(id, material);
                await _unitOfWork.CompleteAsync();
                var updatedMaterial = await _unitOfWork.MaterialRepository.GetByIdAsync(id, m => m.Materialfiles, m => m.Materiallinks);
                return Result.Success<MaterialOutDTO, Error>(MaterialOutDTO.FromMaterial(updatedMaterial!));
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<MaterialOutDTO, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.MaterialRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> DeleteLinkAsync(int linkId)
        {
            try
            {
                await _unitOfWork.MateriallinkRepository.DeleteAsync(linkId);
                await _unitOfWork.CompleteAsync();
                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }

        public async Task<Result<string, Error>> RemoveRangeAsync(List<int> entities)
        {
            if (entities.Count == 0)
            {
                return Result.Failure<string, Error>(Errors.General.NotRecords());
            }
            await _unitOfWork.MaterialRepository.RemoveRange(entities);
            await _unitOfWork.CompleteAsync();
            return Result.Success<string, Error>("Deleted was successful");
        }

        public async Task<Result<string, Error>> DeleteFileAsync(int fileId)
        {
            try
            {
                var materialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(fileId);
                if (materialFile is not null && materialFile.MaterialFile is not null)
                {
                    await _fileHelper.DeleteFileAsync(materialFile.MaterialFile);
                }

                await _unitOfWork.MaterialfileRepository.DeleteAsync(fileId);
                await _unitOfWork.CompleteAsync();

                return Result.Success<string, Error>("Deleted was successful");
            }
            catch (KeyNotFoundException)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
        }
        public async Task<Result<MaterialfileOutDTO, Error>> AddFileAsync(IFormFile file, int id)
        {
            var fileLink = await _fileHelper.AddFileAsync(file);
            Materialfile materialfile = new()
            {
                MaterialId = id,
                MaterialFile = fileLink
            };
            var addedFile = await _unitOfWork.MaterialfileRepository.AddAsync(materialfile);
            await _unitOfWork.CompleteAsync();

            return Result.Success<MaterialfileOutDTO, Error>(MaterialfileOutDTO.FromMaterialFile(addedFile));
        }

        public async Task<Result<Materiallink, Error>> AddLinkAsync(string link, int id)
        {
            Materiallink materialLink = new()
            {
                MaterialId = id,
                MaterialLink = link
            };
            var addedLink = await _unitOfWork.MateriallinkRepository.AddAsync(materialLink);
            await _unitOfWork.CompleteAsync();

            if (addedLink.MaterialLink is not null)
            {
                return Result.Success<Materiallink, Error>(addedLink);
            }
            else
            {
                return Result.Failure<Materiallink, Error>(Errors.General.NotAdded());
            }
        }

        public async Task<Result<MaterialOutDTO, Error>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.MaterialRepository.GetByIdAsync(id, m => m.Materialfiles, m => m.Materiallinks); 
            if (entity is null)
            {
                return Result.Failure<MaterialOutDTO, Error>(Errors.General.NotFound());
            }
            return Result.Success<MaterialOutDTO, Error>(MaterialOutDTO.FromMaterial(entity));
        }

        public async Task<Result<string, Error>> GetFileByIdAsync(int id)
        {
            var materialFile = await _unitOfWork.MaterialfileRepository.GetByIdAsync(id);

            if (materialFile is null || materialFile.MaterialFile is null)
            {
                return Result.Failure<string, Error>(Errors.General.NotFound());
            }
            return Result.Success<string, Error>(await _fileHelper
                .GetFileLink(materialFile.MaterialFile));
        }

        public async Task<IEnumerable<MaterialOutDTO>> GetAllByCourseAsync(int id)
        {
            var materials = await _unitOfWork.MaterialRepository
                .GetAllByCourseAsync(m => m.CourseId == id);
            return materials.Select(MaterialOutDTO.FromMaterial).ToList();
        }
    }
}
