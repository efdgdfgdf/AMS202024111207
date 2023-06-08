using Microsoft.AspNetCore.Mvc;

using AMS202024111207.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMS202024111207.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssetController : Controller
    {
        private readonly AmsDbContext _context;
        private IList<Asset> assets;

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

        //资产详情
        public IActionResult AssetDetails(int id)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.AssetId == id);
            return View(asset);
        }


        //回显资产信息
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
                        //删除资产旧的相片
                        System.IO.File.Delete($"{_path}\\{asset.ImgName}");
                        //相片提交
                        string fileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(imgFile.FileName).Substring(1)}";
                        string savePath = $"{_path}\\{fileName}";
                        using (var steam = new FileStream(savePath, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(steam);
                        }
                        int assetId = asset.AssetId;
                        var temp = _context.Assets.FirstOrDefault(a => a.AssetId == assetId);
                        if (temp != null)
                        {
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
                        }
                        else
                        {
                            TempData["Result"] = "此固定资产信息不存在!";
                        }
                        return RedirectToAction("AssetAdmin");//重定向到资产管理页
                    }
                }
                else
                {
                    TempData["Result"] = "固定资产信息修改失败!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    TempData["error"] = error.ErrorMessage;
                }

                int assetId = asset.AssetId;
                var temp = _context.Assets.FirstOrDefault(a => a.AssetId == assetId);
                if (temp != null)
                {
                    temp.AssetName = asset.AssetName;
                    temp.Specification = asset.Specification;
                    temp.Price = asset.Price;
                    temp.PurchaseDate = asset.PurchaseDate;
                    temp.Location = asset.Location;
                    temp.CategoryId = asset.CategoryId;
                    temp.ImgName = asset.ImgName;
                    temp.CustodianId = asset.CustodianId;
                    _context.SaveChanges();
                    TempData["Result"] = "固定资产信息修改成功!";
                }
                else
                {
                    TempData["Result"] = "此固定资产信息不存在!";
                }
                return RedirectToAction("AssetAdmin");//重定向到资产管理页
            }
            return View(asset);
        }

        //AssetAdmin Action: 固定资产管理页面
        public IActionResult AssetAdmin(string keyword, string field, string sortOrder, int? page)
        {
            IQueryable<Asset> query = _context.Assets
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

        //删除资产信息
        public IActionResult AssetDelete(int id)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.AssetId == id);
            try
            {
                _context.Assets.Remove(asset);
                _context.SaveChanges();
                //删除资产相片
                System.IO.File.Delete($"{_path}\\{asset.ImgName}");
                TempData["Result"] = "固定资产信息删除成功!";
            }
            catch (Exception)
            {
                TempData["Result"] = "固定资产信息删除失败!";
            }
            return RedirectToAction("AssetAdmin"); //重定向到资产管理页
        }
    }
}
