using Microsoft.AspNetCore.Mvc;

using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class QueryController : Controller
    {
        private readonly AmsDbContext _context;
        private IList<Asset> assets;
        private IList<Category> categories;
        private IList<Department> departments;
        private IList<Employee> employees;
        private string _path; //图片路径变项
        public QueryController(AmsDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }

        //QueryAssetAdmin Action: 固定资产管理页面
        public IActionResult QueryAsset(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                assets = _context.Assets.OrderBy(a => a.AssetId)
                    .Where(a => a.AssetId.ToString().Contains(keyword) || a.AssetName.Contains(keyword) || a.Specification.Contains(keyword) || a.PurchaseDate.ToString().Contains(keyword) || a.Location.Contains(keyword) || a.Category.CategoryName.Contains(keyword) || a.Custodian.UserName.Contains(keyword) || a.Custodian.Department.DepartmentName.Contains(keyword))
                    .Include(a => a.Category).AsNoTracking()
                    .Include(a => a.Category).AsNoTracking()
                    .Include(a => a.Custodian).AsNoTracking()
                    .Include(a => a.Custodian.Department).AsNoTracking()
                    .ToList();
                ViewBag.keyword = keyword;
                ViewBag.assets = assets;
                return View(assets);
            }
            assets = _context.Assets.OrderBy(a => a.AssetId)
            .Include(a => a.Category).AsNoTracking()
            .Include(a => a.Custodian).AsNoTracking()
            .Include(a => a.Custodian.Department).AsNoTracking()
            .ToList();
            ViewBag.assets = assets;
            return View(assets);
        }

        //QueryCategoryAdmin Action: 固定资产类别管理页面
        public IActionResult QueryCategory(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                categories = _context.Categories.OrderBy(c => c.CategoryId)
                    .Where(c => c.CategoryId.ToString().Contains(keyword) || c.CategoryName.Contains(keyword) || c.Description.Contains(keyword))
                    .ToList();
                ViewBag.keyword = keyword;
                ViewBag.categories = categories;
                return View(categories);
            }
            categories = _context.Categories.OrderBy(c => c.CategoryId).ToList();
            ViewBag.categories = categories;
            return View(categories);
        }


        //QueryDepartmentAdmin Action: 部门管理页面
        public IActionResult QueryDepartment(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                departments = _context.Departments.OrderBy(d => d.DepartmentId)
                    .Where(d => d.DepartmentId.ToString().Contains(keyword) || d.DepartmentName.Contains(keyword) || d.Supervisor.UserName.Contains(keyword))
                    .Include(d => d.Supervisor).AsNoTracking()
                    .ToList();
                ViewBag.keyword = keyword;
                ViewBag.departments = departments;
                return View(departments);
            }
            departments = _context.Departments.OrderBy(d => d.DepartmentId)
            .Include(d => d.Supervisor).AsNoTracking()
            .ToList();
            ViewBag.departments = departments;
            return View(departments);
        }


        //QueryEmployeeAdmin Action: 员工管理页面
        public IActionResult QueryEmployee(string keyword)
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
    }
}
