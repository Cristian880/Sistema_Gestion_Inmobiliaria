using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sis_Inmobiliaria.WebApp.Controllers
{
    [Authorize(Roles = "GeneralPublic")]
    public class GeneralPublicHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
