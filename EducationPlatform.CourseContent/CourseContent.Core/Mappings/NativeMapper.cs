using CourseContent.Core.DTO.Requests;
using CourseContent.Core.DTO.Responses;
using CourseContent.Domain.Entities;

namespace CourseContent.Core.Mappings
{
    internal static class NativeMapper
    {
        #region *** Assignment and related ***
        public static Assignmentfile ToAssignmentFile(int id, string fileLink)
        {
            return new Assignmentfile
            {
                AssignmentId = id,
                AssignmentFile = fileLink
            };
        }
        public static Assignmentlink ToAssignmentLink(string link, int id)
        {
            return new Assignmentlink
            {
                AssignmentId = id,
                AssignmentLink = link
            };
        }

        public static AssignmentfileOutDTO FromAssignmentFile(Assignmentfile assignment)
        {
            return new AssignmentfileOutDTO
            {
                Id = assignment.Id,
                AssignmentFile = assignment.AssignmentFile
            };
        }

        public static AssignmentlinkOutDTO FromAssignmentLink(Assignmentlink assignmentLink)
        {
            return new AssignmentlinkOutDTO
            {
                Id = assignmentLink.Id,
                AssignmentLink = assignmentLink.AssignmentLink
            };
        }

        public static Assignment ToAssignment(AssignmentUpdateDTO assignmentDto)
        {
            return new Assignment
            {
                Id = assignmentDto.Id,
                CourseId = assignmentDto.CourseId,
                TopicId = assignmentDto.TopicId,
                AssignmentName = assignmentDto.AssignmentName,
                AssignmentDescription = assignmentDto.AssignmentDescription,
                MaxMark = assignmentDto.MaxMark,
                MinMark = assignmentDto.MinMark,
                IsRequired = assignmentDto.IsRequired,
                AssignmentDatePublication = assignmentDto.AssignmentDatePublication,
                AssignmentDeadline = assignmentDto.AssignmentDeadline,
                IsEdited = assignmentDto.IsEdited,
                EditedTime = assignmentDto.EditedTime
            };
        }

        public static Assignment ToAssignment(AssignmentDTO assignmentDto)
        {
            return new Assignment
            {
                CourseId = assignmentDto.CourseId,
                TopicId = assignmentDto.TopicId,
                AssignmentName = assignmentDto.AssignmentName,
                AssignmentDescription = assignmentDto.AssignmentDescription,
                MaxMark = assignmentDto.MaxMark,
                MinMark = assignmentDto.MinMark,
                IsRequired = assignmentDto.IsRequired,
                AssignmentDatePublication = assignmentDto.AssignmentDatePublication,
                AssignmentDeadline = assignmentDto.AssignmentDeadline
            };
        }

        public static AssignmentOutDTO FromAssignment(Assignment? assignment)
        {
            return new AssignmentOutDTO
            {
                Id = assignment.Id,
                TopicId = assignment.TopicId,
                AssignmentName = assignment.AssignmentName,
                AssignmentDescription = assignment.AssignmentDescription,
                AssignmentDatePublication = assignment.AssignmentDatePublication,
                AssignmentDeadline = assignment.AssignmentDeadline,
                MaxMark = assignment.MaxMark,
                MinMark = assignment.MinMark,
                IsRequired = assignment.IsRequired,
                IsEdited = assignment.IsEdited,
                EditedTime = assignment.EditedTime,
                Assignmentfiles = assignment
                    .Assignmentfiles.Select(af => FromAssignmentFile(af)).ToList(),
                Assignmentlinks = assignment
                    .Assignmentlinks.Select(al => FromAssignmentLink(al)).ToList()
            };
        }
        #endregion

        #region *** Material and related ***
        public static Materialfile ToMaterialFile(int id, string fileLink)
        {
            return new Materialfile
            {
                MaterialId = id,
                MaterialFile = fileLink
            };
        }
        public static Materiallink ToMaterialLink(string link, int id)
        {
            return new Materiallink
            {
                MaterialId = id,
                MaterialLink = link
            };
        }

        public static MaterialfileOutDTO FromMaterialFile(Materialfile materialfile)
        {
            return new MaterialfileOutDTO
            {
                Id = materialfile.Id,
                MaterialFile = materialfile.MaterialFile
            };
        }

        public static MateriallinkOutDTO FromMaterialLink(Materiallink materiallink)
        {
            return new MateriallinkOutDTO
            {
                Id = materiallink.Id,
                MaterialLink = materiallink.MaterialLink
            };
        }

        public static Material ToMaterial(MaterialUpdateDTO materialDto)
        {
            return new Material
            {
                Id = materialDto.Id,
                CourseId = materialDto.CourseId,
                TopicId = materialDto.TopicId,
                MaterialName = materialDto.MaterialName,
                MaterialDescription = materialDto.MaterialDescription,
                IsEdited = materialDto.IsEdited,
                EditedTime = materialDto.EditedTime
            };
        }

        public static Material ToMaterial(MaterialDTO materialDto)
        {
            return new Material
            {
                CourseId = materialDto.CourseId,
                TopicId = materialDto.TopicId,
                MaterialName = materialDto.MaterialName,
                MaterialDescription = materialDto.MaterialDescription,
                MaterialDatePublication = materialDto.MaterialDatePublication
            };
        }

        public static MaterialOutDTO FromMaterial(Material? material)
        {
            return new MaterialOutDTO
            {
                Id = material.Id,
                TopicId = material.TopicId,
                MaterialName = material.MaterialName,
                MaterialDescription = material.MaterialDescription,
                MaterialDatePublication = material.MaterialDatePublication,
                IsEdited = material.IsEdited,
                EditedTime = material.EditedTime,
                Materialfiles = material.Materialfiles
                    .Select(mf => FromMaterialFile(mf)).ToList(),
                Materiallinks = material.Materiallinks
                    .Select(ml => FromMaterialLink(ml)).ToList()
            };
        }
        #endregion

        public static TopicOutDTO FromTopic(Topic? topic)
        {
            return new TopicOutDTO
            {
                CourseId = topic.CourseId,
                Id = topic.Id,
                Title = topic.Title
            };
        }

        public static Topic ToTopic(TopicUpdateDTO topic)
        {
            return new Topic
            {
                Id = topic.Id,
                CourseId = topic.CourseId,
                Title = topic.Title
            };
        }

        public static Topic ToTopic(TopicDTO topic)
        {
            return new Topic
            {
                CourseId = topic.CourseId,
                Title = topic.Title
            };
        }
    }
}