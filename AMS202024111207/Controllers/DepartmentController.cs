using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly AmsDbContext _context;
        private IList<Department> departments;

        /*  部门的CRUD  */
        //提交资产的表单
        public IActionResult DepartmentAdd()
        {
            return View();
        }

        //DepartmentAdd Action:负责提交部门信息
        [HttpPost]
        public IActionResult DepartmentAdd(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    TempData["Result"] = "添加部门信息成功!";
                    return RedirectToAction("DepartmentAdmin");//重定向到部门管理页
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "添加部门信息失败，信息有误!";
                }
            }
            return View(department);
        }

        //修改部门信息
        public IActionResult DepartmentEdit(int id)
        {
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentId == id);
            return View(department);
        }

        //修改部门信息
        [HttpPost]
        public IActionResult DepartmentEdit(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int departmentId = department.DepartmentId;
                    var temp = _context.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);
                    temp.DepartmentName = department.DepartmentName;
                    temp.SupervisorId = department.SupervisorId;
                    _context.SaveChanges();
                    TempData["Result"] = "部门信息修改成功!";
                    return RedirectToAction("DepartmentAdmin");//重定向到部门管理页
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "部门信息修改失败!";
                }
            }
            return View(department);
        }

        //DepartmentAdmin Action: 部门管理页面
        public IActionResult DepartmentAdmin(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                departments = _context.Departments.OrderBy(d => d.DepartmentId)
                    .Where(c => c.DepartmentName.Contains(keyword) || c.SupervisorId.Contains(keyword))
                    .ToList();
                ViewBag.keyword = keyword;
                return View(departments);
            }
            departments = _context.Departments.OrderBy(d => d.DepartmentId)
            .Include(d => d.Supervisor).AsNoTracking()
            .ToList();
            return View(departments);
        }

        //删除部门信息
        public IActionResult DepartmentDelete(int id)
        {
            var departments = _context.Departments.FirstOrDefault(d => d.DepartmentId == id);
            _context.Departments.Remove(departments);
            _context.SaveChanges();
            TempData["Result"] = "部门信息删除成功!";
            return RedirectToAction("DepartmentAdmin"); //重定向到部门管理页
        }
    }
}
