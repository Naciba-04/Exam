using FinalExam.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalExam.ViewModels.Products;

public class ProductCreateVM
{
    [Required,MaxLength(32),MinLength(9)]
    public string FullName { get; set; }
    [Required]
    public IFormFile ProfileImage { get; set; }
    [Required]
    public int DepartmentId { get; set; }
}
