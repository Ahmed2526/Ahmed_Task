using Ahmed_Task.Data;
using Ahmed_Task.Enums;
using Ahmed_Task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ahmed_Task.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class PatientsController : Controller
    {
        private readonly MedLinkDbContext _context;

        public PatientsController(MedLinkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var patients = await _context.Patients.Select(e => new PatientVM
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Phone = e.Phone,
                IsLockedOut = e.IsLockedOut
            }).ToListAsync();

            return View(patients);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _context.Patients.Select(e => new PatientVM
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.BirthDate,
                IsLockedOut = e.IsLockedOut
            }).FirstOrDefaultAsync(e => e.Id == id);

            if (patient is null)
                return NotFound();

            return View(patient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Patient not found!"
                });
            }

            patient.IsLockedOut = !patient.IsLockedOut;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = patient.IsLockedOut
                    ? "Patient Locked Successfully!"
                    : "Patient Unlocked Successfully!"
            });
        }
    }
}
