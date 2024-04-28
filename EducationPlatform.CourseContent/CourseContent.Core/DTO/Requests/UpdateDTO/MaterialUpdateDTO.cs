using CourseContent.Core.DTO.CommonValidation;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.DTO.Requests.UpdateDTO
{
    public class MaterialUpdateDTO
    {
        public int Id { get; set; }

        public int? TopicId { get; set; }

        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; } = DateTime.UtcNow;

        [ValidateFile([".png",
            ".jpg",
            ".jpeg",
            ".doc",
            ".pdf",
            ".docx",
            ".pptx",
            ".ppt",
            ".xls",
            ".xlsx"],
            ErrorMessage = "Файл має непідтримуване розширення")]
        public List<IFormFile>? MaterialFiles { get; set; }

        public List<string>? MaterialLinks { get; set; }

        public static Material FromMaterialUpdateDto(MaterialUpdateDTO materialDto)
        {
            return new Material
            {
                Id = materialDto.Id,    
                TopicId = materialDto.TopicId,
                MaterialName = materialDto.MaterialName,
                MaterialDescription = materialDto.MaterialDescription,
                MaterialDatePublication = materialDto.MaterialDatePublication
            };
        }
    }
}
