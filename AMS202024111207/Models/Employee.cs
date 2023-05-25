using System;
using System.Collections.Generic;

namespace AMS202024111207.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
