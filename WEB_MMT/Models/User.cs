using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_MMT.Models;

public partial class User
{
    public User()
    {
        this.Options = new HashSet<Option>();
    }

    [Display(Name = "User ID")]
    public int Iduser { get; set; }
    [Display(Name = "User Name")]
    [Required(ErrorMessage = "*")]
    public string Nameuser { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "*")]
    public string Email { get; set; } = null!;
    [Display(Name = "Password")]
    [Required(ErrorMessage = "*")]
    public string Passwords { get; set; } = null!;

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
}
