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
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    TempData["Result"] = "添加部门信息成功!";
                    return RedirectToAction("EmployeeAdmin");//重定向到部门管理页
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "添加部门信息失败，信息有误!";
                }
            }
            return View(employee);
        }

        //修改员工信息
        public IActionResult EmployeeEdit(string id)
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
                    string employeeId = employee.EmployeeId;
                    var temp = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                    temp.Password = employee.Password;
                    temp.Name = employee.Name;
                    temp.Phone = employee.Phone;
                    temp.Role = employee.Role;
                    temp.DepartmentId = employee.DepartmentId;
                    _context.SaveChanges();
                    TempData["Result"] = "员工信息修改成功!";
                    return RedirectToAction("EmployeeAdmin");//重定向到员工管理页
                }
                catch (Exception ex)
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
                    .Where(e => e.Name.Contains(keyword) || e.Phone.Contains(keyword) || e.Role.Contains(keyword))
                    .ToList();
                ViewBag.keyword = keyword;
                return View(employees);
            }
            employees = _context.Employees.OrderBy(e => e.EmployeeId)
            .Include(e => e.Department).AsNoTracking()
            .ToList();
            return View(employees);
        }

        //删除员工信息
        public IActionResult EmployeeDelete(string id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            _context.Employees.Remove(employees);
            _context.SaveChanges();
            TempData["Result"] = "员工信息删除成功!";
            return RedirectToAction("EmployeeAdmin"); //重定向到员工管理页
        }
    }
}
