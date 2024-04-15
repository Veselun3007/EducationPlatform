using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class MaterialOutDTO
    {
        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? EditedTime { get; set; }

        public ICollection<MaterialfileOutDTO>? Materialfiles { get; set; }

        public ICollection<MateriallinkOutDTO>? Materiallinks { get; set; }

        public static MaterialOutDTO FromMaterial(Material material)
        {
            return new MaterialOutDTO
            {
                MaterialName = material.MaterialName,
                MaterialDescription = material.MaterialDescription,
                MaterialDatePublication = material.MaterialDatePublication,
                IsEdited = material.IsEdited,
                EditedTime = material.EditedTime,
                Materialfiles = material.Materialfiles
                    .Select(mf => MaterialfileOutDTO
                    .FromMaterialFile(mf)).ToList(),
                Materiallinks = material.Materiallinks
                    .Select(ml => MateriallinkOutDTO
                    .FromMaterialLink(ml)).ToList()
            };
        }
    }
}
