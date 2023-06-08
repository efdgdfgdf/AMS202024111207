using Microsoft.AspNetCore.Mvc;

using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult QueryAsset(string keyword, string field, string sortOrder, int? page)
        {
            IQueryable<Asset> query = _context.Assets
                .OrderBy(a => a.AssetId)
                .Include(a => a.Category).AsNoTracking()
                .Include(a => a.Custodian).AsNoTracking()
                .Include(a => a.Custodian.Department).AsNoTracking();

            // 针对资产编号、价格、购入日期进行排序
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "AssetIdDesc":
                        query = query.OrderByDescending(a => a.AssetId);
                        ViewBag.SortOrder = "AssetIdDesc";
                        break;
                    case "AssetIdAsc":
                        query = query.OrderBy(a => a.AssetId);
                        ViewBag.SortOrder = "AssetIdAsc";
                        break;
                    case "PriceDesc":
                        query = query.OrderByDescending(a => a.Price);
                        ViewBag.SortOrder = "PriceDesc";
                        break;
                    case "PriceAsc":
                        query = query.OrderBy(a => a.Price);
                        ViewBag.SortOrder = "PriceAsc";
                        break;
                    case "PurchaseDateDesc":
                        query = query.OrderByDescending(a => a.PurchaseDate);
                        ViewBag.SortOrder = "PurchaseDateDesc";
                        break;
                    case "PurchaseDateAsc":
                        query = query.OrderBy(a => a.PurchaseDate);
                        ViewBag.SortOrder = "PurchaseDateAsc";
                        break;
                    default:
                        query = query.OrderBy(a => a.AssetId);
                        ViewBag.SortOrder = "AssetIdAsc";
                        break;
                }
            }


            // 针对单个字段进行关键词搜素
            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "AssetId":
                        query = query.Where(a => a.AssetId.ToString().Equals(keyword));
                        break;
                    case "AssetName":
                        query = query.Where(a => a.AssetName.Contains(keyword));
                        break;
                    case "Specification":
                        query = query.Where(a => a.Specification.Contains(keyword));
                        break;
                    case "Price":
                        query = query.Where(a => a.Price.ToString().Contains(keyword));
                        break;
                    case "PurchaseDate":
                        query = query.Where(a => a.PurchaseDate.ToString().Contains(keyword));
                        break;
                    case "Location":
                        query = query.Where(a => a.Location.Contains(keyword));
                        break;
                    case "Category":
                        query = query.Where(a => a.Category.CategoryName.Contains(keyword));
                        break;
                    case "Custodian":
                        query = query.Where(a => a.Custodian.UserName.Contains(keyword));
                        break;
                    case "Department":
                        query = query.Where(a => a.Custodian.Department.DepartmentName.Contains(keyword));
                        break;
                    default:
                        query = query.Where(a =>
                            a.AssetId.ToString().Equals(keyword) ||
                            a.AssetName.Contains(keyword) ||
                            a.Specification.Contains(keyword) ||
                            a.Price.ToString().Contains(keyword) ||
                            a.PurchaseDate.ToString().Contains(keyword) ||
                            a.Location.Contains(keyword) ||
                            a.Category.CategoryName.Contains(keyword) ||
                            a.Custodian.UserName.Contains(keyword) ||
                            a.Custodian.Department.DepartmentName.Contains(keyword)
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
            assets = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // 是否有资产数据
            //assets = query.ToList();
            if (assets != null && assets.Count > 0)
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
              new SelectListItem { Value = "AssetId", Text = "资产编号" },
              new SelectListItem { Value = "AssetName", Text = "资产名称" },
              new SelectListItem { Value = "Specification", Text = "规格型号" },
              new SelectListItem { Value = "Price", Text = "价格" },
              new SelectListItem { Value = "PurchaseDate", Text = "购入日期" },
              new SelectListItem { Value = "Location", Text = "存放地点" },
              new SelectListItem { Value = "Category", Text = "资产类别" },
              new SelectListItem { Value = "Custodian", Text = "资产保管人" },
              new SelectListItem { Value = "Department", Text = "所属部门" }
            };
            ViewData["options"] = new SelectList(options, "Value", "Text");

            // 关键词和查询字段
            ViewBag.keyword = keyword;
            ViewBag.field = field;
            ViewBag.page = page;
            ViewBag.totalPages = totalPages;
            ViewBag.totalItemCount = totalItemCount;

            return View(assets);
        }

        //QueryCategoryAdmin Action: 固定资产类别管理页面
        public IActionResult QueryCategory(string keyword)
        {
            bool hasResult;
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                categories = _context.Categories.OrderBy(c => c.CategoryId)
                    .Where(c => c.CategoryId.ToString().Equals(keyword) || c.CategoryName.Contains(keyword) || c.Description.Contains(keyword))
                    .ToList();
                ViewBag.keyword = keyword;
                // 是否有资产类别数据
                hasResult = (categories != null && categories.Count > 0);
                ViewBag.result = hasResult;
                return View(categories);
            }
            categories = _context.Categories.OrderBy(c => c.CategoryId).ToList();
            // 是否有资产类别数据
            hasResult = (categories != null && categories.Count > 0);
            ViewBag.result = hasResult;
            return View(categories);
        }


        //QueryDepartmentAdmin Action: 部门管理页面
        public IActionResult QueryDepartment(string keyword)
        {
            bool hasResult;
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                departments = _context.Departments.OrderBy(d => d.DepartmentId)
                    .Where(d => d.DepartmentId.ToString().Equals(keyword) || d.DepartmentName.Contains(keyword) || d.Supervisor.UserName.Contains(keyword))
                    .Include(d => d.Supervisor).AsNoTracking()
                    .ToList();
                ViewBag.keyword = keyword;
                // 是否有部门数据
                hasResult = (departments != null && departments.Count > 0);
                ViewBag.result = hasResult;
                return View(departments);
            }
            departments = _context.Departments.OrderBy(d => d.DepartmentId)
            .Include(d => d.Supervisor).AsNoTracking()
            .ToList();
            // 是否有部门数据
            hasResult = (departments != null && departments.Count > 0);
            ViewBag.result = hasResult;
            return View(departments);
        }


        //QueryEmployeeAdmin Action: 员工管理页面
        public IActionResult QueryEmployee(string keyword, string field, int? page)
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
    }
}
