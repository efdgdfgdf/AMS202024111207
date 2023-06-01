using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS202024111207.Models;

public partial class Department
{

    [Display(Name = "部门名称")]
    public int DepartmentId { get; set; }

    [Display(Name = "部门名称")]
    [Required(ErrorMessage = "必须填写")]
    public string DepartmentName { get; set; } = null!;

    [Display(Name = "部门主管编号")]
    public int? SupervisorId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Employee? Supervisor { get; set; }
}
