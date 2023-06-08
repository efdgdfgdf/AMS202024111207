using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                    TempData["Result"] = "添加员工信息失败，此用户名或联络电话已存在，换一个吧!";
                }
            }
            return View(employee);
        }
        //员工详情
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
        public IActionResult EmployeeAdmin(string keyword, string field, int? page)
        {
            IQueryable<Employee> query = _context.Employees
               .OrderBy(e => e.EmployeeId)
               .Include(a => a.Department).AsNoTracking();

            // 针对单个字段进行关键词搜素
            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "EmployeeId":
                        query = query.Where(e => e.EmployeeId.ToString().Equals(keyword));
                        break;
                    case "UserName":
                        query = query.Where(e => e.UserName.Contains(keyword));
                        break;
                    case "Password":
                        query = query.Where(e => e.Password.Contains(keyword));
                        break;
                    case "Name":
                        query = query.Where(e => e.Name.Contains(keyword));
                        break;
                    case "Phone":
                        query = query.Where(e => e.Phone.Contains(keyword));
                        break;
                    case "JoinDate":
                        query = query.Where(e => e.JoinDate.ToString().Contains(keyword));
                        break;
                    case "Role":
                        query = query.Where(e => e.Role.Contains(keyword));
                        break;
                    case "Department":
                        query = query.Where(e => e.Department.DepartmentName.Contains(keyword));
                        break;
                    default:
                        query = query.Where(e =>
                            e.EmployeeId.ToString().Equals(keyword) ||
                            e.UserName.Contains(keyword) ||
                            e.Password.Contains(keyword) ||
                            e.Name.Contains(keyword) ||
                            e.Phone.Contains(keyword) ||
                            e.JoinDate.ToString().Contains(keyword) ||
                            e.Role.Contains(keyword) ||
                            e.Department.DepartmentName.Contains(keyword)
                        );
                        break;
                }
            }

            // 分页
            int pageSize = 10; // 每页显示的数量
            int pageNumber = (page ?? 1); // 当前页数
            int totalItemCount = query.Count();
            // 计算总页数
            int totalPages = (int)Math.Ceiling((double)totalItemCount / pageSize);

            // 如果请求的页码超出范围，则显示第一页或最后一页
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages));

            // 获取当前页的资产信息
            employees = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // 是否有资产数据
            //employees = query.ToList();
            if (employees != null && employees.Count > 0)
            {
                ViewBag.result = true;
            }
            else
            {
                ViewBag.result = false;
            }

            // 传递查询字段给前端视图
            var options = new List<SelectListItem> {
              new SelectListItem { Value = "All", Text = "全部" },
              new SelectListItem { Value = "EmployeeId", Text = "员工编号" },
              new SelectListItem { Value = "UserName", Text = "用户名" },
              new SelectListItem { Value = "Password", Text = "密码" },
              new SelectListItem { Value = "Name", Text = "姓名" },
              new SelectListItem { Value = "Phone", Text = "联络电话" },
              new SelectListItem { Value = "JoinDate", Text = "注册日期" },
              new SelectListItem { Value = "Role", Text = "角色" },
              new SelectListItem { Value = "Department", Text = "所属部门" }
            };
            ViewData["options"] = new SelectList(options, "Value", "Text");

            // 关键词和查询字段
            ViewBag.keyword = keyword;
            ViewBag.field = field;
            ViewBag.page = page;
            ViewBag.totalPages = totalPages;
            ViewBag.totalItemCount = totalItemCount;

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
                TempData["Result"] = "员工信息删除失败！请先删除该资产保管人管理的所有资产 或 删除他主管的部门";
            }
            return RedirectToAction("EmployeeAdmin"); //重定向到员工管理页
        }
    }
}
