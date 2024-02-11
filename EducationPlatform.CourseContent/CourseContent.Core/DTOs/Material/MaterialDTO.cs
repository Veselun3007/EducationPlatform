using CourseContent.Core.DTOs.CommonValidation;
using CourseContent.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CourseContent.Core.DTOs
{
    public class MaterialDTO
    {

        public int CourseId { get; set; }

        public string MaterialName { get; set; } = null!;

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; } = DateTime.UtcNow;

        [ValidateFile([".png", ".jpg", ".jpeg", ".doc", ".pdf", ".docx"], ErrorMessage = "Файл має непідтримуване розширення")]
        public List<IFormFile>? MaterialFiles { get; set; }

        public static Material FromMaterialDto(MaterialDTO materialDto)
        {
            return new Material
            {
                CourseId = materialDto.CourseId,
                MaterialName = materialDto.MaterialName,
                MaterialDescription = materialDto.MaterialDescription,
                MaterialDatePublication = materialDto.MaterialDatePublication
            };
        }
    }
}
