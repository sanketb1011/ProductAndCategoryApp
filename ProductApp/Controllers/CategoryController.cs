using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;
using System.Linq;

namespace ProductApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductContext _context;
        public CategoryController(ProductContext dbcontext)
        {
            _context = dbcontext;
        }
        public IActionResult Index()
        {
            var categories=_context.Categories.ToList();

            return View(categories);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
       [HttpPost]
        public IActionResult AddCategory(Category ct) 
        {
            if (ModelState.IsValid)
            {
                var cat = new Category { Cid = 0, Name = ct.Name };

                _context.Categories.Add(cat);
                _context.SaveChanges();
            }
            else 
            {
                TempData["error"] = "Enter the details";
            }
            return RedirectToAction("Index");
        }
        public IActionResult DeleteCat(int id) 
        {
            var category = _context.Categories.FirstOrDefault(e=> e.Cid==id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditCat(int id) 
        {
            var category=_context.Categories.FirstOrDefault(e => e.Cid==id);
            var result = new Category { Cid = category.Cid, Name = category.Name };
            return View(result);
            
        }

        [HttpPost]
        public IActionResult EditCat(Category ct) 
        {
            var category = new Category { Cid = ct.Cid, Name = ct.Name };

            _context.Categories.Update(category);
            _context.SaveChanges(); 

            return RedirectToAction("Index");       
        }
    }
}
