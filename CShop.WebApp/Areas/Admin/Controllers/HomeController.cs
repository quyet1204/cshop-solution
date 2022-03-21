using Microsoft.AspNetCore.Mvc;

namespace CShop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
