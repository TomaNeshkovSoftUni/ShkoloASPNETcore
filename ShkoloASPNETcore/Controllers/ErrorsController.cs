using Microsoft.AspNetCore.Mvc;

namespace ShkoloASPNETcore.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        public IActionResult Error401()
        {
            Response.StatusCode = 401;
            return View();
        }

        public IActionResult Error400()
        {
            Response.StatusCode = 400;
            return View();
        }

        public IActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}