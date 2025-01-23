using FinalExam.Models.Commons;

namespace FinalExam.Models;

public class Product:BaseEntity
{
    public string FullName { get; set; }
    public string ProfileImageUrl { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}
