using Microsoft.CodeAnalysis.Elfie.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Drawing.Printing;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FinalExam.ViewModels.Auths;

public class LoginVM
{
    [Required]
    public string UserNameOrEmail { get; set; }
    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
}
