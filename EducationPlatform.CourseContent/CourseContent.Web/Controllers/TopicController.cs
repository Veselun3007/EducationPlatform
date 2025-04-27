using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Core.Interfaces;
using CourseContent.Web.Controllers.Base;

namespace CourseContent.Web.Controllers
{

    public class TopicController : ContentController<TopicDTO, TopicOutDTO, TopicUpdateDTO>
    {
        public TopicController(IContentService<TopicDTO, TopicOutDTO, TopicUpdateDTO> service) : base(service) { }       
    }
}
