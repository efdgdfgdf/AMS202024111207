using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024111207.Models;

public partial class Asset
{
    [Display(Name = "资产编号")]
    public int AssetId { get; set; }

    [Display(Name = "资产名称")]
    [Required(ErrorMessage = "必须填写")]
    public string AssetName { get; set; } = null!;

    [Display(Name = "资产规格")]
    [StringLength(100)]
    [Required(ErrorMessage = "必须填写")]
    public string Specification { get; set; } = null!;

    [Display(Name = "价格")]
    [Range(0, 5000000, ErrorMessage = "资产单价必须介于0 - 5000000元")]
    public decimal Price { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "购入日期")]
    public DateTime PurchaseDate { get; set; }

    [Display(Name = "存放位置")]
    [Required(ErrorMessage = "必须填写")]
    public string Location { get; set; } = null!;

    [Display(Name = "类别编号")]
    public int? CategoryId { get; set; }

    [Display(Name = "资产图片名称")]
    public string? ImgName { get; set; }

    [Display(Name = "资产保管人编号")]
    public int? CustodianId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Employee? Custodian { get; set; }
}
