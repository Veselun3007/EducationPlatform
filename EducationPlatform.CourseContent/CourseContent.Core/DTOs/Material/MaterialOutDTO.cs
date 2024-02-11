using CourseContent.Core.DTOs.CommonValidation;
using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTOs
{
    public class MaterialOutDTO
    {
        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; }

        [ValidateFile([".png", ".jpg", ".jpeg", ".doc", ".pdf", ".docx"], ErrorMessage = "Зображення має непідтримуване розширення")]
        public ICollection<Materialfile>? Materialfiles { get; set; }

        public static MaterialOutDTO FromMaterial(Material material)
        {
            return new MaterialOutDTO
            {
                MaterialName = material.MaterialName,
                MaterialDescription = material.MaterialDescription,
                MaterialDatePublication = material.MaterialDatePublication,
                Materialfiles = material.Materialfiles.ToList()
            };
        }
    }
}
