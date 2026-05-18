using Ahmed_Task.Data;
using Ahmed_Task.Enums;
using Ahmed_Task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ahmed_Task.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class DoctorsController : Controller
    {
        private readonly MedLinkDbContext _context;

        public DoctorsController(MedLinkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors.Select(e => new DoctorVM
            {
                Id = e.Id,
                Name = e.FirstName + " " + e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Speciality = e.Speciality.Name
            }).ToListAsync();

            return View(doctors);
        }


    }
}
