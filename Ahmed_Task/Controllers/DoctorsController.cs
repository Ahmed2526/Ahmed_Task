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
                Speciality = e.Speciality.Name,
                IsLockedOut = e.IsLockedOut
            }).ToListAsync();

            return View(doctors);
        }

        [HttpGet]
        //do each part seperately using ajax on demand to avoid loading data that may never be accessed.
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _context.Doctors.Select(e => new DoctorVM
            {
                Id = e.Id,
                Name = e.FirstName + " " + e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Speciality = e.Speciality.Name,
                IsLockedOut = e.IsLockedOut,
                Clinics = e.Clinics.Select(c => new ClinicVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Speciality = c.Speciality.Name,
                    Location = c.Location.Governate.Name + " - " + c.Location.City.Name,
                    Phone = c.Phone,
                    Price = c.Price
                }).ToList()
            }).FirstOrDefaultAsync(e => e.Id == id);

            if (doctor is null)
                return NotFound();

            //Select doctor Schedule
            doctor.DoctorAvailablilities = await _context.DoctorAvailabilities.Where(e => e.DoctorId == id).Select(e => new DoctorAvailablilityVM
            {
                Id = e.Id,
                Day = e.Day,
                AppointmentStart = e.AppointmentStart,
                AppointmentEnd = e.AppointmentEnd,
                Clinic = e.Clinic.Name
            }).ToListAsync();

            //Select doctor Appointments
            doctor.Appointment = await _context.Appointments.Where(e => e.DoctorId == id).Select(e => new AppointmentVM
            {
                Id = e.Id,
                Patient = e.Patient.Name,
                Clinic = e.Clinic.Name,
                AppointmentStart = e.AppointmentStart,
                AppointmentEnd = e.AppointmentEnd,
                Status = ((AppointmentStatus)e.Status).ToString(),
                Day = e.Day,
            }).ToListAsync();


            return View(doctor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Doctor not found!"
                });
            }

            doctor.IsLockedOut = !doctor.IsLockedOut;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = doctor.IsLockedOut
                    ? "Doctor Locked Successfully!"
                    : "Doctor Unlocked Successfully!"
            });
        }


    }
}
