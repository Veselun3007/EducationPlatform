using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class MaterialfileOutDTO
    {
        public int Id { get; set; }

        public string? MaterialFile { get; set; }

        public static MaterialfileOutDTO FromMaterialFile(Materialfile materialfile)
        {
            return new MaterialfileOutDTO
            {
                Id = materialfile.Id,
                MaterialFile = materialfile.MaterialFile
            };
        }
    }
}
