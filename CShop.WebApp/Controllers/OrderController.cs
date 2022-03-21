using CShop.Data.DataContext;
using CShop.Data.Entities;
using CShop.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CShop.WebApp.Controllers
{
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> userManager;

        public OrderController(DataDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [Route("")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListOrders()
        {
            var items = await (from order in _context.Orders
                               orderby order.Id descending
                               select new ListOrdersViewModel()
                               {
                                   OrderId = order.Id,
                                   OrderDate = order.OrderDate,
                                   OrderStatus = order.Status,
                                   ShipAddress = order.ShipAddress,
                                   ShipEmail = order.ShipEmail,
                                   ShipName = order.ShipName,
                                   ShipPhoneNumber = order.ShipPhoneNumber,
                                   UserId = order.UserId,
                                   Detail = (from d in _context.OrderDetails
                                             where order.Id == d.OrderId
                                             select new OrderDetailViewModel()
                                             {
                                                 OrderId = d.OrderId,
                                                 Price = d.Price,
                                                 ProductId = d.ProductId,
                                                 Quantity = d.Quantity,
                                                 Id = d.Id,
                                                 Product = (from pr in _context.Products
                                                            where d.ProductId == pr.Id
                                                            select pr).FirstOrDefault(),
                                             }).AsEnumerable()
                               }).ToListAsync();
            return View(items);
        }

        [Route("checkout")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUserId = await userManager.GetUserAsync(User);

            ViewBag.OrderItems = await (from c in _context.Carts
                                        join pr in _context.Products on c.ProductId equals pr.Id
                                        where c.UserId == currentUserId.Id
                                        orderby c.DateCreated descending
                                        select new ListCartViewModel()
                                        {
                                            ProductId = pr.Id,
                                            ProductName = pr.Name,
                                            ProductImage = pr.UrlImage,
                                            CartId = decimal.ToInt32(c.Id),
                                            CartPrice = c.Price,
                                            CartQuantity = c.Quantity,
                                            CartTotal = decimal.ToInt32((decimal)c.Price) * c.Quantity,
                                            ProductSlug = pr.Slug
                                        }).ToListAsync();

            ViewBag.UserInfo = await _context.AppUsers.FirstOrDefaultAsync(p => p.Id == currentUserId.Id);

            return View();
        }


        [Route("create-order")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            var currentUserId = await userManager.GetUserAsync(User);

            var carts = await _context.Carts.Where(c => c.UserId == currentUserId.Id).ToListAsync();

            Order item = new Order()
            {
                OrderDate = DateTime.Now,
                ShipAddress = model.ShipAddress,
                ShipName = model.ShipName,
                ShipEmail = model.ShipEmail,
                ShipPhoneNumber = model.ShipPhoneNumber,
                UserId = currentUserId.Id,
                Status = 1
            };
            _context.Orders.Add(item);
            await _context.SaveChangesAsync();

            foreach (var cart in carts)
            {
                OrderDetail detail = new OrderDetail()
                {
                    OrderId = item.Id,
                    Price = (decimal)cart.Price * cart.Quantity,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                };
                _context.OrderDetails.Add(detail);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    // order xong tru kho
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cart.ProductId);
                    product.Quantity -= cart.Quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    // order xong clear gio hang
                    var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cart.Id);
                    _context.Carts.Remove(cartItem);
                    await _context.SaveChangesAsync();

                    //return RedirectToAction("Index");
                }
            }

            return RedirectToAction("ListOrders");

        }

        [Route("cancel-order/{orderId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(ord => ord.Id == orderId);
            order.Status = 0;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("delete-order/{orderId}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(ord => ord.Id == orderId);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
