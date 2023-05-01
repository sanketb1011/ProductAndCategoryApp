using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProductApp.Data;
using ProductApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProductApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext dbcontext)
        {
            _context = dbcontext;
        }
        public IActionResult Index(int? page)
        {
            var pd = _context.Products;
            var ct=_context.Categories;
            var result = from p in pd
                         join c in ct
                         on p.Cid equals c.Cid

                         select new ProductView
                         {
							 Productid = p.Pid,
                             ProductName = p.Name,
                             CatId = c.Cid,
                             CatName = c.Name
                         };
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var finalresult= result.ToPagedList(pageNumber, pageSize);
            return View(finalresult);
        }

        public IActionResult AddProduct() 
        {
            var result=_context.Categories.ToList();
            List<string> catlist=new List<string>();
            foreach (var cat in result) 
            {
                catlist.Add(cat.Name);
            
            }

            ViewBag.categorylist =catlist;

            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(ProductView productView) 
        {
            
            var result = _context.Categories.FirstOrDefault(model => model.Name == productView.CatName);

            Product NewProduct = new Product { Name = productView.ProductName, Cid = result.Cid };
            _context.Products.Add(NewProduct);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Deleteproduct(int id) 
        {
            var result = _context.Products.FirstOrDefault(m => m.Pid == id);
            _context.Products.Remove(result);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Editproduct(int id)
            
        {
            var product = _context.Products.FirstOrDefault(m => m.Pid == id);
            ProductView pdv = new ProductView()
            {
                Productid = product.Pid,
                ProductName = product.Name,
                CatId = product.Cid,
            };
            var result = _context.Categories.ToList();
            List<string> catlist = new List<string>();
            foreach (var cat in result)
            {
                catlist.Add(cat.Name);

            }

            ViewBag.categorylist = catlist;

            return View(pdv);          
        }
        [HttpPost]
        public IActionResult Editproduct(ProductView productView) 
        {
            var category = _context.Categories.FirstOrDefault(m => m.Name == productView.CatName);
            Product product = new Product() { Pid = productView.Productid, Name = productView.ProductName, Cid = category.Cid };
            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
