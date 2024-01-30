﻿using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTOs
{
    public class AssignmentOutDTO
    {
        public required string AssignmentName { get; set; }

        public string? AssignmentDescription { get; set; }

        public DateTime AssignmentDatePublication { get; set; }

        public DateTime AssignmentDeadline { get; set; }

        public ICollection<Assignmentfile>? Assignmentfiles { get; set; }

        public static AssignmentOutDTO FromAssignment(Assignment assignment)
        {
            return new AssignmentOutDTO
            {
                AssignmentName = assignment.AssignmentName,
                AssignmentDescription = assignment.AssignmentDescription,
                AssignmentDatePublication = assignment.AssignmentDatePublication,
                AssignmentDeadline = assignment.AssignmentDeadline,
                Assignmentfiles = assignment.Assignmentfiles.ToList()
            };
        }

    }
}
