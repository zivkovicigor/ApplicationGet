using ApplicationGet.Data;
using ApplicationGet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationGet.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _db;

        public UserController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return Redirect("Identity/Account/Register?ReturnUrl=%2F");
        }
    }
}
