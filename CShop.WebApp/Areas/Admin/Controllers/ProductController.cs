using CShop.Common;
using CShop.Data.DataContext;
using CShop.Data.Entities;
using CShop.Data.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/product")]  
    public class ProductController : Controller
        {
            private readonly DataDbContext _context;

            public ProductController(DataDbContext context)
            {
                _context = context;
            }

            [Route("index")]
            [Route("")]
            public async Task<IActionResult> Index()
            {
                var item = _context.Products.OrderByDescending(p => p.Id).ToList();
                return View(item);
            }

            [Route("create")]
            public IActionResult Create()
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                return View();
            }

            [HttpPost]
            [Route("create")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CreateProductViewModel model)
            {
                if (ModelState.IsValid)
                {
                    string POST_IMAGE_PATH = "products/images/";

                    if (model.UrlImage != null)
                    {

                        var image = UploadImage.UploadImageFile(model.UrlImage, POST_IMAGE_PATH);

                        Product item = new Product()
                        {
                            Name = model.Name,
                            UrlImage = image,
                            Slug = TextHelper.ToUnsignString(model.Name).ToLower(),
                            Description = model.Description,
                            Price = model.Price,
                            OriginalPrice = model.OriginalPrice,
                            Details = model.Details,
                            Quantity = model.Quantity,
                            Stock = model.Stock,
                            CategoryId = model.CategoryId,
                            Status = 1
                        };

                        _context.Add(item);
                        await _context.SaveChangesAsync();

                    }


                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }

            [Route("update/{id}")]
            public IActionResult Update(int id)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                var item = _context.Products.Where(s => s.Id == id).First();
                return View(item);
            }

            [Route("update/{id}")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Update(int id, UpdateProductViewModel model)
            {
                Product item = _context.Products.Where(s => s.Id == id).First();
                item.Name = model.Name;
                item.Description = model.Description;
                item.Price = model.Price;
                item.OriginalPrice = model.OriginalPrice;
                item.Details = model.Details;
                item.Quantity = model.Quantity;
                item.Stock = model.Stock;
                item.CategoryId = model.CategoryId;
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Product");
            }

            [Route("update-image/{id}")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> UpdateImage(int id, IFormFile UrlImage)
            {
                string POST_IMAGE_PATH = "products/images/";

                if (UrlImage != null)
                {

                    var image = UploadImage.UploadImageFile(UrlImage, POST_IMAGE_PATH);

                    Product item = _context.Products.Where(s => s.Id == id).First();
                    item.UrlImage = image;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                    return Redirect("/admin/product/update/" + id);
                }
                return Redirect("/admin/product/update/" + id);

            }

            [HttpGet("delete/{id}")]
            public IActionResult Delete(int id)
            {
                try
                {
                    Product item = _context.Products.Where(s => s.Id == id).First();
                    _context.Products.Remove(item);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Index");
                }

            }

            [HttpPost("delete-selected")]
            public async Task<IActionResult> DeleteSelected(int[] listDelete)
            {
                foreach (int id in listDelete)
                {
                    var item = await _context.Products.FindAsync(id);
                    _context.Products.Remove(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
}
