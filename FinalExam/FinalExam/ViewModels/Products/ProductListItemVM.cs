using FinalExam.Models;

namespace FinalExam.ViewModels.Products;

public class ProductListItemVM
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string ProfileImageUrl { get; set; }
    public Department Department { get; set; }
}
