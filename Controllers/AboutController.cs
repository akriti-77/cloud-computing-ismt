using Microsoft.AspNetCore.Mvc;

namespace ISMTCollege.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
