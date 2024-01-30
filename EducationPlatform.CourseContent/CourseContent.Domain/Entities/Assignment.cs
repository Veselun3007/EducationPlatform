﻿using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Assignment : IAggregateRoot
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public required string AssignmentName { get; set; }

    public string? AssignmentDescription { get; set; }

    public required DateTime AssignmentDatePublication { get; set; }

    public required DateTime AssignmentDeadline { get; set; }

    public virtual ICollection<Assignmentfile>? Assignmentfiles { get; set; } = new List<Assignmentfile>();

    public virtual Course? Course { get; set; } = null!;
}
