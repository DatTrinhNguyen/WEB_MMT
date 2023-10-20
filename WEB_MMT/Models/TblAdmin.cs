using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_MMT.Models;

public partial class TblAdmin
{
    public TblAdmin()
    {
        this.TblCategories = new HashSet<TblCategory>();
    }
    public int Adid { get; set; }

    [Display(Name = "Admin Name")]
    [Required(ErrorMessage = "*")]
    public string Adname { get; set; } = null!;

    [Display(Name = "Password")]
    [Required(ErrorMessage = "*")]
    public string Adpass { get; set; } = null!;

    public virtual ICollection<TblCategory> TblCategories { get; set; } = new List<TblCategory>();
}
