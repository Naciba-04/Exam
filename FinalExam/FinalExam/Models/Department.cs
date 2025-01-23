using FinalExam.Models.Commons;

namespace FinalExam.Models;

public class Department:BaseEntity
{
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}
