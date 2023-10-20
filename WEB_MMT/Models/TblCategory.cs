using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace WEB_MMT.Models;

public partial class TblCategory
{
    public TblCategory()
    {
        this.TblQuestions = new HashSet<TblQuestion>();
    }
    public int CatId { get; set; }

    public string CatName { get; set; } = null!;

    public int? adid { get; set; }
    public string cat_encrytped_string { get; set; }
    public virtual TblAdmin? FKadid { get; set; }
    public virtual ICollection<TblQuestion> TblQuestions { get; set; }
    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
}
