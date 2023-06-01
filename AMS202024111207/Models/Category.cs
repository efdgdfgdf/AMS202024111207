using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024111207.Models;

public partial class Category
{
    [Display(Name = "类别编号")]
    public int CategoryId { get; set; }

    [Display(Name = "类别名称")]
    [Required(ErrorMessage = "必须填写")]
    public string CategoryName { get; set; } = null!;

    [Display(Name = "类别说明 (选填)")]
    public string? Description { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
