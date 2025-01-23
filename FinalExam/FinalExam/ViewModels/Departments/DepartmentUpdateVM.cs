using System.ComponentModel.DataAnnotations;

namespace FinalExam.ViewModels.Departments;

public class DepartmentUpdateVM
{
    [Required,MaxLength(32)]
    public string Name { get; set; }
}
