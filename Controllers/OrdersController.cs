using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult RecentlyOrders()
        {
            return View();
        }
    }
}
