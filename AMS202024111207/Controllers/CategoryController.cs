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
                catch (Exception ex)
                {
                    TempData["Result"] = "添加固定资产类别信息失败，信息有误!";
                }
            }
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
                    temp.CategoryName = category.CategoryName;
                    temp.Description = category.Description;
                    _context.SaveChanges();
                    TempData["Result"] = "固定资产类别信息修改成功!";
                    return RedirectToAction("CategoryAdmin");//重定向到资产类别管理页
                }
                catch (Exception ex)
                {
                    TempData["Result"] = "固定资产类别信息修改失败!";
                }
            }
            return View(category);
        }

        //CategoryAdmin Action: 固定资产类别管理页面
        public IActionResult CategoryAdmin(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                categories = _context.Categories.OrderBy(c => c.CategoryId)
                    .Where(c => c.CategoryName.Contains(keyword) || c.Description.Contains(keyword))
                    .ToList();
                ViewBag.keyword = keyword;
                return View(categories);
            }
            categories = _context.Categories.OrderBy(c => c.CategoryId).ToList();
            return View(categories);
        }

        //删除资产类别信息
        public IActionResult CategoryDelete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["Result"] = "固定资产类别信息删除成功!";
            return RedirectToAction("CategoryAdmin"); //重定向到资产管理页
        }
    }
}
