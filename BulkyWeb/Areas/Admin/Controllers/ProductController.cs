using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; //Replaced IProductRepository _productRepo to IUnitOfWork
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //    .GetAll().Select(u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            return View(objProductList);
        }
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category SelectListItem is a class in .NET (specifically within the Microsoft.AspNetCore.Mvc.Rendering or System.Web.Mvc namespaces) used to represent a single item within an HTML <select> (dropdown or list box) element
            //   .GetAll().Select(u => new SelectListItem
            //   {
            //       Text = u.Name,
            //       Value = u.Id.ToString() 
            //   });
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList; // before using ViewModel
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }),
                Product = new Product()
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
           
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj);
                //_db.SaveChanges();
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                 return RedirectToAction("Index");
            }
            else {//Re-populate CategoryList in case of validation errors
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            return View(productVM);
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id==0)
            {
                return NotFound();
            }
            //Product? productFromDb = _db.Categories.Find(id);
            Product? productFromDb = _unitOfWork.Product.Get(u=>u.Id==id);
            //Product? productFromDb1 = _db.Categories.FirstOrDefault(u => u.Id==id);
            //Product? productFromDb2 = _db.Categories.Where(u => u.Id==id).FirstOrDefault();
            if (productFromDb == null) { 
                 return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj);
                //_db.SaveChanges();
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            //Product? productFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Product? productFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

    }
}


