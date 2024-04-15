using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class MateriallinkOutDTO
    {
        public int Id { get; set; }

        public string? MaterialLink { get; set; }

        public static MateriallinkOutDTO FromMaterialLink(Materiallink materiallink)
        {
            return new MateriallinkOutDTO
            {
                Id = materiallink.Id,
                MaterialLink = materiallink.MaterialLink
            };
        }
    }
}
