using AMS202024111207.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Metrics;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin,Member")]
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

        // 登录和注册
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
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
               // TempData["Message"] = employee.Role.ToString();
                return RedirectToAction("Index");

            }
            TempData["Message"] = "账号或密码有错! 请重试❌";
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //写入日期
                    employee.JoinDate = DateTime.Today;
                   
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    TempData["RegisterResult"] = "注册成功！请登录";
                    return RedirectToAction("Login");
                }
                catch (Exception)
                {
                    TempData["RegisterResult"] = "注册失败，此用户名或联络电话已存在，换一个吧!";
                }
            }
            else
            {
                TempData["RegisterResult"] = "注册失败，输入信息不规范，请稍后重试！";
            }
            return RedirectToAction("Login");
        }


        // 注销
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }


        //重置密码
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ResetPassword(string username, string newPassword, string confirmPassword)
        {
            // 根据用户名查询用户
            var employee = _context.Employees.FirstOrDefault(e => e.UserName == username.Trim());

            // 如果找到了用户，则进行密码重置操作
            if (employee != null)
            {
                // 判断两次密码是否一致
                if (newPassword != confirmPassword)
                {
                    TempData["Message"] = "两次密码不一致，请重新输入";
                    return View("ResetPassword");
                }

                // 判断新密码是否符合要求
                if (!Regex.IsMatch(newPassword, @"^\s*(?=.*?\d)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[^\w\s]).{8,}\s*$"))
                {
                    TempData["Message"] = "新密码必须包含数字、小写字母、大写字母和特殊字符，且长度不少于 8 位";
                    return View("ResetPassword");
                }

                // 修改密码并保存到数据库中
                employee.Password = newPassword;
                _context.Employees.Update(employee);
                _context.SaveChanges();
                TempData["Message"] = "密码重置成功，可以登录啦！";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Message"] = "找不到该用户名对应的账号，请重新输入";
            }
            return View("ResetPassword");
        }



        // 显示个人信息资料
        //public IActionResult Users()
        //{
        //    var employee = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.UserName.Equals(User.Identity.Name));
        //    return View(employee);
        //}
        public IActionResult Users(int id)
        {
            var employees = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.EmployeeId == id);
            return View(employees);
        }

        //编辑个人信息资料
        public IActionResult Settings(int id)
        {
            var employees = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            return View(employees);
        }

        //编辑个人信息资料
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
                        // 判断用户名和密码是否被修改
                        bool isCredentialsModified = temp.UserName != employee.UserName || !string.IsNullOrEmpty(employee.Password);
                        if (isCredentialsModified)
                        {
                            temp.UserName = employee.UserName;
                            temp.Password = employee.Password;
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
                            TempData["Result"] = "更新成功！用户名或密码被更改，需重新登录";
                            return RedirectToAction("Logout");
                        }
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
                    return RedirectToAction("Settings", "Home",new { id = employeeId});
                }
                catch (Exception)
                {
                    TempData["Result"] = "更新失败，换个用户名或手机号试试吧!";
                }
            }
            return View(employee);
        }

        // 关于页面
        public IActionResult About()
        {
            employees = _context.Employees.OrderBy(e => e.EmployeeId)
                .Include(e => e.Department).AsNoTracking()
                .OrderByDescending(e => e.JoinDate)
                .Take(8)
                .ToList();
            var catNames = _context.Categories.Select(c => c.CategoryName).ToList(); //取出固定资产所有的类别名称
            ViewBag.catNames = catNames; //传给前端
            return View(employees);
        }
    }
}
