using Microsoft.AspNetCore.Mvc;

using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssetController : Controller
    {
        private readonly AmsDbContext _context;
        private IList<Asset> assets;
        private IList<Category> categories;
        private IList<Department> departments;
        private IList<Employee> employees;
        private string _path; //图片路径变项
        public AssetController(AmsDbContext context, IHostEnvironment environment)
        {
            _context = context;
            //设置相片的文件夹路径,透过构造方法取得
            _path = environment.ContentRootPath + "//wwwroot//images";
        }

        /*  资产的CRUD  */
        //提交资产的表单
        public IActionResult AssetAdd()
        {
            return View();
        }
        //AssetAdd Action:负责提交资产
        [HttpPost]
        public async Task<IActionResult> AssetAdd(Asset asset, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    if (imgFile.Length > 0)
                    {
                        //相片提交
                        string fileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(imgFile.FileName).Substring(1)}";
                        string savePath = $"{_path}\\{fileName}";
                        using (var steam = new FileStream(savePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(steam);
                        }
                        //相片信息写入记录
                        asset.ImgName = fileName;
                        asset.PurchaseDate = DateTime.Today;
                        _context.Assets.Add(asset);
                        _context.SaveChanges();
                        TempData["Result"] = "添加固定资产信息成功!";
                        return RedirectToAction("AssetAdmin");//重定向到资产管理页
                    }
                    else
                    {
                        TempData["Result"] = "添加固定资产信息失败，信息有误!";
                    }
                }
            }
            return View(asset);
        }

        //修改资产信息
        public IActionResult AssetEdit(int id)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.AssetId == id);
            return View(asset);
        }

        //修改资产信息
        [HttpPost]
        public async Task<IActionResult> AssetEdit(Asset asset, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    if (imgFile.Length > 0)
                    {
                        //相片提交
                        string fileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(imgFile.FileName).Substring(1)}";
                        string savePath = $"{_path}\\{fileName}";
                        using (var steam = new FileStream(savePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(steam);
                        }
                        int assetId = asset.AssetId;
                        var temp = _context.Assets.FirstOrDefault(a => a.AssetId == assetId);
                        temp.AssetName = asset.AssetName;
                        temp.Specification = asset.Specification;
                        temp.Price = asset.Price;
                        temp.PurchaseDate = asset.PurchaseDate;
                        temp.Location = asset.Location;
                        temp.CategoryId = asset.CategoryId;
                        temp.ImgName = fileName;
                        temp.CustodianId = asset.CustodianId;
                        _context.SaveChanges();
                        TempData["Result"] = "固定资产信息修改成功!";
                        return RedirectToAction("AssetAdmin");//重定向到资产管理页
                    }
                }
            }
            else
            {
                TempData["Result"] = "固定资产信息修改失败!";
            }
            return View(asset);
        }

        //AssetAdmin Action: 固定资产管理页面
        public IActionResult AssetAdmin(string keyword)
        {
            if (keyword != null)
            {
                //使用LINQ扩充方法，可多个字段查询
                assets = _context.Assets.OrderBy(a => a.AssetId)
                    .Where(a => a.AssetName.Contains(keyword) || a.Specification.Contains(keyword) || a.Location.Contains(keyword) || a.CustodianId.Contains(keyword))
                    .Include(a => a.Category).AsNoTracking()
                    .ToList();
                ViewBag.keyword = keyword;
                return View(assets);
            }
            assets = _context.Assets.OrderBy(a => a.AssetId)
            .Include(a => a.Category).AsNoTracking()
            .Include(a => a.Custodian).AsNoTracking()
            .ToList();
            return View(assets);
        }

        //删除资产信息
        public IActionResult AssetDelete(int id)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.AssetId == id);
            _context.Assets.Remove(asset);
            _context.SaveChanges();
            //删除资产相片
            System.IO.File.Delete($"{_path}\\{asset.ImgName}");
            TempData["Result"] = "固定资产信息删除成功!";
            return RedirectToAction("AssetAdmin"); //重定向到资产管理页
        }
        
    }
}
