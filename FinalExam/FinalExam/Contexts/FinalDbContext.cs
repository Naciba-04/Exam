using FinalExam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace FinalExam.Contexts;

public class FinalDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Product>Products { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public FinalDbContext(DbContextOptions<FinalDbContext> options) : base(options)
    {
    }   
}
