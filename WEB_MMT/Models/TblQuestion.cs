using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_MMT.Models;

public partial class TblQuestion
{
    public int Questionid { get; set; }

    [Display(Name = "Nội dung câu hỏi")]
    [Required(ErrorMessage ="*")]
    public string Questiontext { get; set; } = null!;

    [Display(Name = "Câu A")]
    [Required(ErrorMessage = "*")]
    public string QuestionA { get; set; } = null!;

    [Display(Name = "Câu B")]
    [Required(ErrorMessage = "*")]
    public string QuestionB { get; set; } = null!;

    [Display(Name = "Câu C")]
    [Required(ErrorMessage = "*")]
    public string QuestionC { get; set; } = null!;

    [Display(Name = "Câu D")]
    [Required(ErrorMessage = "*")]
    public string QuestionD { get; set; } = null!;

    [Display(Name = "Câu Trả Lời")]
    [Required(ErrorMessage = "*")]
    public string QuestionCorrect { get; set; } = null!;

    [Display(Name = "Bộ Câu Hỏi")]
    [Required(ErrorMessage = "*")]
    public int? cat_id { get; set; }

    public virtual TblCategory? FKcat_id { get; set; }

}
