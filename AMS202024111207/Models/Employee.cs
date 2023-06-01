using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024111207.Models;

public partial class Employee
{
    [Display(Name = "员工编号")]
    public int EmployeeId { get; set; }

    [Display(Name = "用户名")]
    [Required(ErrorMessage = "必须填写")]
    public string UserName { get; set; } = null!;

    [Display(Name = "密码")]
    [Required(ErrorMessage = "必须填写")]
    [RegularExpression(@"^\s*(?=.*?\d)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[^\w\s]).{8,}\s*$", ErrorMessage = "密码必须包含数字、小写字母、大写字母和特殊字符，且长度不少于 8 位")]
    public string Password { get; set; } = null!;

    [Display(Name = "姓名")]
    [Required(ErrorMessage = "必须填写")]
    public string Name { get; set; } = null!;

    [Display(Name = "联络电话")]
    [Required(ErrorMessage = "必须填写")]
    [RegularExpression(@"^\s*1[3-9]\d{9}\s*$", ErrorMessage = "请输入有效的手机号码")]
    public string Phone { get; set; } = null!;

    [Display(Name = "QQ邮箱 (选填)")]
    [EmailAddress]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@qq\.com$", ErrorMessage = "请输入有效的电子邮箱地址(@qq.com)")]
    public string? Qqemail { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "注册日期")]
    public DateTime JoinDate { get; set; }

    [Display(Name = "角色")]
    [Required(ErrorMessage = "必须填写")]
    public string Role { get; set; } = null!;

    [Display(Name = "关于我 (选填)")]
    public string? About { get; set; }

    [Display(Name = "部门编号")]
    public int? DepartmentId { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
