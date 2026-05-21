using Ahmed_Task.Areas.Receptionist.ViewModel;
using Ahmed_Task.Data;
using Ahmed_Task.Enums;
using Ahmed_Task.Models;
using Ahmed_Task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ahmed_Task.Areas.Receptionist.Controllers
{
    [Area("Receptionist")]
    [Authorize(Roles = Roles.Receptionist)]
    public class DoctorController : Controller
    {
        private readonly MedLinkDbContext _MedLinkDbContext;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public DoctorController(MedLinkDbContext context, ApplicationDbContext applicationDbContext)
        {
            _MedLinkDbContext = context;
            _ApplicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var receptionistId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var receptionistDocIds = _ApplicationDbContext.ReceptionistDoctors
                .Where(rd => rd.ReceptionistId == receptionistId)
                .Select(rd => rd.DoctorId)
                .ToList();

            var doctors = await _MedLinkDbContext.Doctors
                .Where(d => d.IsLockedOut == false && receptionistDocIds.Contains(d.Id))
                .Select(d => new DoctorVM
                {
                    Id = d.Id,
                    Name = d.FirstName + " " + d.LastName,
                    Speciality = d.Speciality.Name,
                    Email = d.Email,
                    Phone = d.Phone,
                    IsLockedOut = d.IsLockedOut,
                }).ToListAsync();

            return View("index", doctors);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var receptionistId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var receptionistDocIds = _ApplicationDbContext.ReceptionistDoctors
               .Where(rd => rd.ReceptionistId == receptionistId)
               .Select(rd => rd.DoctorId)
               .ToList();

            if (!receptionistDocIds.Contains(id))
                return NotFound();

            var doctor = await _MedLinkDbContext.Doctors.Select(e => new DoctorVM
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
            doctor.DoctorAvailablilities = await _MedLinkDbContext.DoctorAvailabilities.Where(e => e.DoctorId == id).Select(e => new DoctorAvailablilityVM
            {
                Id = e.Id,
                Day = e.Day,
                AppointmentStart = e.AppointmentStart,
                AppointmentEnd = e.AppointmentEnd,
                Clinic = e.Clinic.Name
            }).ToListAsync();

            //Select doctor Appointments
            doctor.Appointment = await _MedLinkDbContext.Appointments.Where(e => e.DoctorId == id).Select(e => new AppointmentVM
            {
                Id = e.Id,
                Patient = e.Patient.Name,
                Clinic = e.Clinic.Name,
                AppointmentStart = e.AppointmentStart,
                AppointmentEnd = e.AppointmentEnd,
                Status = ((AppointmentStatus)e.Status).ToString(),
                Day = e.Day,
            }).OrderByDescending(a => a.Day).ToListAsync();


            return View(doctor);
        }

        [HttpGet]
        public async Task<IActionResult> AddClinic(int id)
        {
            var receptionistId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            ClinicFormVM clinicFormVM = new ClinicFormVM
            {
                DoctorId = id,

                Speciality = await _MedLinkDbContext.Specialities
                    .Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString()
                    })
                    .ToListAsync(),

                Location = new LocationVM
                {
                    Governate = await _MedLinkDbContext.Governates
                        .Select(g => new SelectListItem
                        {
                            Text = g.Name,
                            Value = g.Id.ToString()
                        })
                        .ToListAsync(),

                    City = []
                }
            };

            return View(clinicFormVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddClinic(ClinicFormVM clinicFormVM)
        {
            var receptionistId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var receptionistDocIds = _ApplicationDbContext.ReceptionistDoctors
               .Where(rd => rd.ReceptionistId == receptionistId)
               .Select(rd => rd.DoctorId)
               .ToList();

            if (!receptionistDocIds.Contains(clinicFormVM.DoctorId))
                return NotFound();

            var clinic = new Clinic
            {
                Name = clinicFormVM.Name,
                Phone = clinicFormVM.Phone,
                Price = clinicFormVM.Price,
                DoctorId = clinicFormVM.DoctorId,
                SpecialityId = clinicFormVM.SpecialityId,
                Location = new Location()
                {
                    GovernateId = clinicFormVM.Location.GovernateId,
                    CityId = clinicFormVM.Location.CityId,
                    Street = clinicFormVM.Location.Street,
                    PostalCode = clinicFormVM.Location.PostalCode
                }
            };

            await _MedLinkDbContext.Clinics.AddAsync(clinic);
            await _MedLinkDbContext.SaveChangesAsync();

            return RedirectToAction("Details", new { id = clinicFormVM.DoctorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditClinic(int id)
        {
            var receptionistId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var clinic = await _MedLinkDbContext.Clinics
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clinic == null)
                return NotFound();

            // Validate receptionist owns doctor
            var isAssigned = await _ApplicationDbContext
                .ReceptionistDoctors
                .AnyAsync(rd =>
                    rd.ReceptionistId == receptionistId &&
                    rd.DoctorId == clinic.DoctorId);

            if (!isAssigned)
                return NotFound();

            var model = new ClinicFormVM
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Phone = clinic.Phone,
                Price = clinic.Price,
                DoctorId = clinic.DoctorId,
                SpecialityId = clinic.SpecialityId,
                Speciality = await _MedLinkDbContext.Specialities
                    .Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString()
                    })
                    .ToListAsync(),

                Location = new LocationVM
                {
                    Street = clinic.Location.Street,
                    PostalCode = clinic.Location.PostalCode,
                    GovernateId = clinic.Location.GovernateId,
                    CityId = clinic.Location.CityId,
                    Governate = await _MedLinkDbContext.Governates
                        .Select(g => new SelectListItem
                        {
                            Text = g.Name,
                            Value = g.Id.ToString()
                        })
                        .ToListAsync(),

                    City = await _MedLinkDbContext.Cities
                        .Where(c =>
                            c.GovernateId ==
                            clinic.Location.GovernateId)
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        })
                        .ToListAsync()
                }
            };

            return View("EditClinic", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClinic(ClinicFormVM clinicFormVM)
        {
            if (!ModelState.IsValid)
            {
                clinicFormVM.Speciality =
                    await _MedLinkDbContext.Specialities
                    .Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString()
                    })
                    .ToListAsync();

                clinicFormVM.Location.Governate =
                    await _MedLinkDbContext.Governates
                    .Select(g => new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    })
                    .ToListAsync();

                clinicFormVM.Location.City =
                    await _MedLinkDbContext.Cities
                    .Where(c =>
                        c.GovernateId ==
                        clinicFormVM.Location.GovernateId)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToListAsync();

                return View("EditClinic", clinicFormVM);
            }

            var receptionistId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var clinic = await _MedLinkDbContext.Clinics
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c =>
                    c.Id == clinicFormVM.Id);

            if (clinic == null)
                return NotFound();

            // Security check
            var isAssigned = await _ApplicationDbContext
                .ReceptionistDoctors
                .AnyAsync(rd =>
                    rd.ReceptionistId == receptionistId &&
                    rd.DoctorId == clinic.DoctorId);

            if (!isAssigned)
                return NotFound();

            // Update clinic
            clinic.Name = clinicFormVM.Name;

            clinic.Phone = clinicFormVM.Phone;

            clinic.Price = clinicFormVM.Price;

            clinic.SpecialityId =
                clinicFormVM.SpecialityId;

            // Update location
            clinic.Location.Street =
                clinicFormVM.Location.Street;

            clinic.Location.PostalCode =
                clinicFormVM.Location.PostalCode;

            clinic.Location.GovernateId =
                clinicFormVM.Location.GovernateId;

            clinic.Location.CityId =
                clinicFormVM.Location.CityId;

            await _MedLinkDbContext.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = clinic.DoctorId });
        }


        [HttpGet]
        public async Task<IActionResult> GetCities(int governateId)
        {
            var cities = await _MedLinkDbContext.Cities
                .Where(c => c.GovernateId == governateId)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name
                })
                .ToListAsync();

            return Json(cities);
        }


        [HttpGet]
        public async Task<IActionResult> AddSchadule(int id)
        {
            var receptionistId =
               User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate receptionist owns doctor
            var isAssigned = await _ApplicationDbContext
                .ReceptionistDoctors
                .AnyAsync(rd =>
                    rd.ReceptionistId == receptionistId &&
                    rd.DoctorId == id);
            if (!isAssigned)
                return NotFound();

            var schaduleFormVM = new SchaduleFormVM
            {
                DoctorId = id,
                Clinics = await _MedLinkDbContext.Clinics
                    .Where(c => c.DoctorId == id)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToListAsync()
            };

            return View(schaduleFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSchadule(SchaduleFormVM schaduleFormVM)
        {
            var receptionistId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate receptionist owns doctor
            var isAssigned = await _ApplicationDbContext
                .ReceptionistDoctors
                .AnyAsync(rd =>
                    rd.ReceptionistId == receptionistId &&
                    rd.DoctorId == schaduleFormVM.DoctorId);

            if (!isAssigned)
                return NotFound();

            // Validate Clinic belongs to doctor
            var clinicExists = await _MedLinkDbContext.Clinics
                .AnyAsync(c =>
                    c.Id == schaduleFormVM.ClinicId &&
                    c.DoctorId == schaduleFormVM.DoctorId);

            if (!clinicExists)
            {
                ModelState.AddModelError(
                    nameof(schaduleFormVM.ClinicId),
                    "Invalid clinic selected.");
            }

            // Validate appointment time
            if (schaduleFormVM.AppointmentStart >=
                schaduleFormVM.AppointmentEnd)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Appointment end time must be greater than start time.");
            }

            // Validate overlapping schedules
            var hasConflict = await _MedLinkDbContext.DoctorAvailabilities
                    .AnyAsync(s =>
                        s.DoctorId == schaduleFormVM.DoctorId &&
                        s.Day == schaduleFormVM.Day &&
                        (
                            schaduleFormVM.AppointmentStart < s.AppointmentEnd &&
                            schaduleFormVM.AppointmentEnd > s.AppointmentStart
                        ));

            if (hasConflict)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This schedule conflicts with an existing schedule.");
            }

            if (!ModelState.IsValid)
            {
                schaduleFormVM.Clinics =
                    await _MedLinkDbContext.Clinics
                    .Where(c => c.DoctorId == schaduleFormVM.DoctorId)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToListAsync();

                return View(schaduleFormVM);
            }

            // Create schedule
            var schadule = new DoctorAvailability
            {
                Day = schaduleFormVM.Day,
                AppointmentStart = schaduleFormVM.AppointmentStart,
                AppointmentEnd = schaduleFormVM.AppointmentEnd,
                DoctorId = schaduleFormVM.DoctorId,
                ClinicId = schaduleFormVM.ClinicId
            };

            await _MedLinkDbContext.DoctorAvailabilities.AddAsync(schadule);

            await _MedLinkDbContext.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = schaduleFormVM.DoctorId });
        }

        [HttpGet]
        public async Task<IActionResult> EditSchadule(int id)
        {
            var receptionistId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var schadule = await _MedLinkDbContext
                .DoctorAvailabilities
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schadule == null)
                return NotFound();

            // Validate receptionist owns doctor
            var isAssigned = await _ApplicationDbContext
                .ReceptionistDoctors
                .AnyAsync(rd =>
                    rd.ReceptionistId == receptionistId &&
                    rd.DoctorId == schadule.DoctorId);

            if (!isAssigned)
                return NotFound();

            var schaduleFormVM = new SchaduleFormVM
            {
                Id = schadule.Id,

                Day = schadule.Day,

                AppointmentStart =
                    schadule.AppointmentStart,

                AppointmentEnd =
                    schadule.AppointmentEnd,

                DoctorId =
                    schadule.DoctorId,

                ClinicId =
                    schadule.ClinicId,

                Clinics = await _MedLinkDbContext.Clinics
                    .Where(c => c.DoctorId == schadule.DoctorId)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToListAsync()
            };

            return View(schaduleFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSchadule(SchaduleFormVM schaduleFormVM)
        {
            var receptionistId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate receptionist owns doctor
            var isAssigned = await _ApplicationDbContext
                .ReceptionistDoctors
                .AnyAsync(rd =>
                    rd.ReceptionistId == receptionistId &&
                    rd.DoctorId == schaduleFormVM.DoctorId);

            if (!isAssigned)
                return NotFound();

            // Validate clinic belongs to doctor
            var clinicExists = await _MedLinkDbContext.Clinics
                .AnyAsync(c =>
                    c.Id == schaduleFormVM.ClinicId &&
                    c.DoctorId == schaduleFormVM.DoctorId);

            if (!clinicExists)
            {
                ModelState.AddModelError(
                    nameof(schaduleFormVM.ClinicId),
                    "Invalid clinic selected.");
            }

            // Validate time
            if (schaduleFormVM.AppointmentStart >=
                schaduleFormVM.AppointmentEnd)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Appointment end time must be greater than start time.");
            }

            // Validate overlapping schedules
            var hasConflict = await _MedLinkDbContext
                .DoctorAvailabilities
                .AnyAsync(s =>
                    s.Id != schaduleFormVM.Id &&
                    s.DoctorId == schaduleFormVM.DoctorId &&
                    s.Day == schaduleFormVM.Day &&
                    (
                        schaduleFormVM.AppointmentStart < s.AppointmentEnd &&
                        schaduleFormVM.AppointmentEnd > s.AppointmentStart
                    ));

            if (hasConflict)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This schedule conflicts with an existing schedule.");
            }

            if (!ModelState.IsValid)
            {
                schaduleFormVM.Clinics =
                    await _MedLinkDbContext.Clinics
                    .Where(c => c.DoctorId == schaduleFormVM.DoctorId)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToListAsync();

                return View(schaduleFormVM);
            }

            var schadule = await _MedLinkDbContext
                .DoctorAvailabilities
                .FirstOrDefaultAsync(s =>
                    s.Id == schaduleFormVM.Id);

            if (schadule == null)
                return NotFound();

            // Update
            schadule.Day = schaduleFormVM.Day;

            schadule.AppointmentStart = schaduleFormVM.AppointmentStart;

            schadule.AppointmentEnd = schaduleFormVM.AppointmentEnd;

            schadule.ClinicId = schaduleFormVM.ClinicId;

            await _MedLinkDbContext.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = schaduleFormVM.DoctorId });
        }
    }
}
