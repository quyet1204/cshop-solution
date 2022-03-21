using CShop.Data.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShop.WebApp.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly DataDbContext _context;

        public ProductController(DataDbContext context)
        {
            _context = context;
        }

        [Route("")]
        public async Task<IActionResult> Index(string catelog, string q)
        {
            ViewBag.ListCategories = _context.Categories.OrderByDescending(c => c.Id).ToList();
            ViewBag.q = q;

            var items = await _context.Products.OrderByDescending(p => p.Id).ToListAsync();

            if (catelog != null && q != null)
            {
                items = await (from c in _context.Categories
                               join pr in _context.Products on c.Id equals pr.CategoryId
                               orderby pr.Id descending
                               where c.Slug == catelog && pr.Name.Contains(q)
                               select pr).ToListAsync();
                return View(items);

            }
            else
            {
                if (catelog != null)
                {
                    ViewBag.Category = _context.Categories.FirstOrDefault(c => c.Slug == catelog);

                    items = await (from c in _context.Categories
                                   join pr in _context.Products on c.Id equals pr.CategoryId
                                   orderby pr.Id descending
                                   where c.Slug == catelog
                                   select pr).ToListAsync();
                    return View(items);
                }

                if (q != null)
                {
                    items = _context.Products.Where(pr => pr.Name.Contains(q)).OrderByDescending(p => p.Id).ToList();
                    return View(items);
                }
            }

            items = await _context.Products.OrderByDescending(p => p.Id).ToListAsync();
            return View(items);
        }

        [Route("{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var item = await _context.Products.Where(p => p.Slug == slug).FirstOrDefaultAsync();
            return View(item);
        }
    }
}
