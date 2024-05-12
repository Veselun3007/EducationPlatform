using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Requests.UpdateDTO
{
    public class MaterialUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? TopicId { get; set; }

        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; } = DateTime.UtcNow;

        public static Material FromMaterialUpdateDto(MaterialUpdateDTO materialDto)
        {
            return new Material
            {
                Id = materialDto.Id,
                CourseId = materialDto.CourseId,
                TopicId = materialDto.TopicId,
                MaterialName = materialDto.MaterialName,
                MaterialDescription = materialDto.MaterialDescription,
                MaterialDatePublication = materialDto.MaterialDatePublication
            };
        }
    }
}
