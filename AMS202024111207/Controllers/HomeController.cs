using AMS202024111207.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;
using System.Data;


namespace AMS202024111207.Controllers
{
    public class HomeController : Controller
    {
        private readonly AmsDbContext _context;
        private IList<Asset> assets;
        private IList<Employee> employees;
        private string _path; //图片路径变项
        public HomeController(AmsDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }

        public IActionResult Index(string cat)
        {
            if (cat != null) //找特定类别的资产相片,最新购入的8张资产相片,作为首页显示用
            {
                assets = _context.Assets.OrderBy(a => a.AssetId)
                .Where(a => a.Category.CategoryName.Equals(cat))
                .Include(a => a.Category).AsNoTracking()
                .OrderByDescending(a => a.PurchaseDate)
                .Take(8)
                .ToList();
            }
            else //找最新提交的8张相片,作为首页显示用
            {
                assets = _context.Assets.OrderBy(a => a.AssetId)
                .Include(a => a.Category).AsNoTracking()
                .OrderByDescending(a => a.PurchaseDate)
                .Take(8)
                .ToList();
            }
            var catNames = _context.Categories.Select(c => c.CategoryName).ToList(); //取出固定资产所有的类别名称
            ViewBag.catNames = catNames; //传给前端
            return View(assets);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string pwd)
        {
            //取得会员对象
            Employee employee = _context.Employees.FirstOrDefault(e => e.UserName == username && e.Password == pwd);
            if (employee != null)
            {

                //建立身份声明
                IList<Claim> claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, employee.UserName),
                        new Claim(ClaimTypes.Role, employee.Role.Trim())
                    };
                //建立身份识别对象,并指定账号与角色
                var claimsIndentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };
                //进行登录动作,并带入身份识别对象
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIndentity), authProperties);
                
                //重定向至会员页
                TempData["Message"] = employee.Role.ToString();
                return RedirectToAction("Index");

            }
            ViewBag.Message = "账号或密码有错! 请重试❌";
            return View("Login");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        // 个人信息
        public IActionResult Users()
        {
            var employees = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.UserName.Equals(User.Identity.Name));
            return View(employees);
        }

        //public IActionResult Users(int id)
        //{
        //    var employees = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.EmployeeId == id);
        //    return View(employees);
        //}

        //设置个人信息
        public IActionResult Settings(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            return View(employees);
        }

        //设置个人信息
        [HttpPost]
        public IActionResult Settings(Employee employee)
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
                        TempData["Result"] = "更新成功!";
                    }
                    return RedirectToAction("Settings", "Home",new { id = employeeId});//重定向到员工管理页
                }
                catch (Exception)
                {
                    TempData["Result"] = "更新失败!";
                }
            }
            return View(employee);
        }
    }
}
