using System;
using System.Collections.Generic;

namespace AMS202024111207.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? SupervisorId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Employee? Supervisor { get; set; }
}
