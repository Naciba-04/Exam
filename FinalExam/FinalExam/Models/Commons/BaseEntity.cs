﻿namespace FinalExam.Models.Commons;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public bool IsDeleted { get; set; }
}
