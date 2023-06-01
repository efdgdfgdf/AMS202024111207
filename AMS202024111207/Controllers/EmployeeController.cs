using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
	{
        private readonly AmsDbContext _context;
        private IList<Employee> employees;

        public EmployeeController(AmsDbContext context, IHostEnvironment environment)
        {
            _context = context;
        }
        /*  员工的CRUD  */
        //提交资产的表单
        public IActionResult EmployeeAdd()
        {
            return View();
        }

        //EmployeeAdd Action:负责提交员工信息
        [HttpPost]
        public IActionResult EmployeeAdd(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //写入日期
                    employee.JoinDate = DateTime.Today;
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    TempData["Result"] = "添加员工信息成功!";
                    return RedirectToAction("EmployeeAdmin");//重定向到员工管理页
                }
                catch (Exception)
                {
                    TempData["Result"] = "添加员工信息失败，此用户名已存在，换一个吧!";
                }
            }
            return View(employee);
        }

        public IActionResult EmployeeDetails(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            return View(employees);
        }

        //修改员工信息
        public IActionResult EmployeeEdit(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            return View(employees);
        }

        //修改员工信息
        [HttpPost]
        public IActionResult EmployeeEdit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int employeeId = employee.EmployeeId;
                    var temp = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                    if (temp != null)
                    {
                        temp.UserName = employee.UserName;
                        temp.Password = employee.Password;
                        temp.Name = employee.Name;
                        temp.Phone = employee.Phone;
                        temp.Qqemail = employee.Qqemail;
                        temp.JoinDate = employee.JoinDate;
                        temp.Role = employee.Role;
                        temp.About = employee.About;
                        temp.DepartmentId = employee.DepartmentId;
                        _context.SaveChanges();
                        TempData["Result"] = "员工信息修改成功!";
                    }
                    else
                    {
                        TempData["Result"] = "员工不存在!";
                    }
                    return RedirectToAction("EmployeeAdmin");//重定向到员工管理页
                }
                catch (Exception)
                {
                    TempData["Result"] = "员工信息修改失败!";
                }
            }
            return View(employee);
        }

        //EmployeeAdmin Action: 员工管理页面
        public IActionResult EmployeeAdmin(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                employees = _context.Employees.OrderBy(e => e.EmployeeId)
                     .Where(e => e.EmployeeId.ToString().Contains(keyword) || e.UserName.Contains(keyword) || e.Name.Contains(keyword) || e.Phone.Contains(keyword) || e.Role.Contains(keyword) || e.Department.DepartmentName.Contains(keyword))
                     .Include(e => e.Department)
                     .ToList();
                ViewBag.keyword = keyword;
                ViewBag.employees = employees;
                return View(employees);
            }
            employees = _context.Employees.OrderBy(e => e.EmployeeId)
            .Include(e => e.Department).AsNoTracking()
            .ToList();
            ViewBag.employees = employees;
            return View(employees);
        }

        //删除员工信息
        public IActionResult EmployeeDelete(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if(employees.UserName == User.Identity.Name)
            {
                TempData["Result"] = "该用户当前在登录中——无法删除！";
                return RedirectToAction("EmployeeAdmin"); //重定向到员工管理页
            }
            try
            {
                _context.Employees.Remove(employees);
                _context.SaveChanges();
                TempData["Result"] = "员工信息删除成功!";
            }
            catch (Exception)
            {
                TempData["Result"] = "员工信息删除失败！请先删除资产保管人--" + employees.UserName + "管理的所有资产 或 删除他主管的部门";
            }
            return RedirectToAction("EmployeeAdmin"); //重定向到员工管理页
        }
    }
}
