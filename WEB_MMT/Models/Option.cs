using System;
using System.Collections.Generic;

namespace WEB_MMT.Models;

public partial class Option
{
    public int OptionId { get; set; }

    public DateTime? OptionDate { get; set; }

    public string OptionName { get; set; } = null!;

    public int? iduser { get; set; }

    public int? users_score { get; set; }
    public int? cat_id { get; set; }

    public virtual User? iduserNavigation { get; set; }
    public virtual TblCategory? catidNavigation { get; set; }
}
