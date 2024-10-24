﻿using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Course : IAggregateRoot
{
    public int Id { get; set; }

    public string CourseName { get; set; } = null!;

    public string? CourseDescription { get; set; }

    public string? CourseLink { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = [];

    public virtual ICollection<Material> Materials { get; set; } = [];

    public virtual ICollection<Topic> Topics { get; set; } = [];
}
