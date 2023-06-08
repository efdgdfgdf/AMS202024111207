using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AmsDbContext _context;
        private IList<Category> categories;

        public CategoryController(AmsDbContext context, IHostEnvironment environment)
        {
            _context = context;
        }

        /*  资产类别的CRUD  */
        //提交资产的表单
        public IActionResult CategoryAdd()
        {
            return View();
        }

        //CategoryAdd Action:负责提交资产类别
        [HttpPost]
        public IActionResult CategoryAdd(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    TempData["Result"] = "添加固定资产类别信息成功!";
                    return RedirectToAction("CategoryAdmin");//重定向到资产类别管理页
                }
                catch (Exception)
                {
                    TempData["Result"] = "添加固定资产类别信息失败!";
                }
            }
            return View(category);
        }
        //资产类别详情
        public IActionResult CategoryDetails(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            return View(category);
        }
        //修改资产信息
        public IActionResult CategoryEdit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            return View(category);
        }

        //修改资产信息
        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int categoryId = category.CategoryId;
                    var temp = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                    if (temp != null)
                    {
                        temp.CategoryName = category.CategoryName;
                        temp.Description = category.Description;
                        _context.SaveChanges();
                        TempData["Result"] = "固定资产类别信息修改成功!";
                    }
                    else
                    {
                        TempData["Result"] = "此资产类别不存在!";
                    }
                    return RedirectToAction("CategoryAdmin");//重定向到资产类别管理页
                }
                catch (Exception)
                {
                    TempData["Result"] = "固定资产类别信息修改失败!";
                }
            }
            return View(category);
        }

        //CategoryAdmin Action: 固定资产类别管理页面
        public IActionResult CategoryAdmin(string keyword)
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

        //删除资产类别信息
        public IActionResult CategoryDelete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["Result"] = "固定资产类别信息删除成功!";
            }
            catch(Exception)
            {
                TempData["Result"] = "固定资产类别信息删除错误！请先清除属于" + category.CategoryName + "的所有资产";
            }
            return RedirectToAction("CategoryAdmin"); //重定向到资产类别管理页
        }
    }
}
