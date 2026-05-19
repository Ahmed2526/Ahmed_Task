using Ahmed_Task.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ahmed_Task.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Receptionist)]
    public class ReceptionistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
