using Ahmed_Task.Data;
using Ahmed_Task.Enums;
using Ahmed_Task.Models;
using Ahmed_Task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ahmed_Task.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ReceptionistsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MedLinkDbContext _medLinkDbContext;
        private readonly ApplicationDbContext _applicationDbContext;
        public ReceptionistsController(UserManager<ApplicationUser> userManager, MedLinkDbContext medLinkDbContext, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _medLinkDbContext = medLinkDbContext;
            _applicationDbContext = applicationDbContext;
        }

        //enhance it using Joins to avoid n+1.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var receptionists = new List<ReceptionistVM>();

            foreach (var user in users)
            {
                {
                    var isReceptionist = await _userManager.IsInRoleAsync(user, Roles.Receptionist);
                    if (isReceptionist)
                    {
                        receptionists.Add(new ReceptionistVM
                        {
                            Id = user.Id,
                            UserName = user.UserName!,
                            Email = user.Email!,
                            PhoneNumber = user.PhoneNumber!,
                            IsLockedOut = user.LockoutEnd > DateTimeOffset.UtcNow
                        });
                    }
                }
            }

            return View(receptionists);
        }

        [HttpGet]
        public async Task<IActionResult> Assign(string id)
        {
            var Doclist = new AssignDocVM()
            {
                ReceptionistId = id,
            };

            Doclist.Doctors = await _medLinkDbContext.Doctors
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FirstName + " " + u.LastName
                }).ToListAsync();


            return View(Doclist);
        }

        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignDocVM vm)
        {
            var doctor = await _medLinkDbContext.Doctors
                .FindAsync(vm.SelectedDoctor);

            if (doctor == null)
                return NotFound();

            var receptionist = await _userManager
                .FindByIdAsync(vm.ReceptionistId);

            if (receptionist == null)
                return NotFound();

            // Check if relation already exists
            var exists = await _applicationDbContext.ReceptionistDoctors
                .AnyAsync(x =>
                    x.DoctorId == doctor.Id &&
                    x.ReceptionistId == receptionist.Id);

            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Doctor already assigned to this receptionist.");

                vm.Doctors = await _medLinkDbContext.Doctors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.FirstName + " " + d.LastName
                    })
                    .ToListAsync();

                return View(vm);
            }

            var receptionistDoctor = new ReceptionistDoctors
            {
                DoctorId = doctor.Id,
                ReceptionistId = receptionist.Id
            };

            await _applicationDbContext.ReceptionistDoctors.AddAsync(receptionistDoctor);

            await _applicationDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var receptionist = await _userManager.FindByIdAsync(id);
            if (receptionist == null)
                return NotFound();

            var receptionistDetails = new ReceptionistVM()
            {
                Id = receptionist.Id,
                Email = receptionist.Email!,
                UserName = receptionist.UserName!,
                PhoneNumber = receptionist.PhoneNumber!,
                IsLockedOut = receptionist.LockoutEnd > DateTimeOffset.UtcNow,
            };

            var docIds = await _applicationDbContext.ReceptionistDoctors
                .Where(rd => rd.ReceptionistId == receptionist.Id)
                .Select(rd => rd.DoctorId)
                .ToListAsync();

            var doctorNames = await _medLinkDbContext.Doctors.Where(d => docIds.Contains(d.Id))
                .Select(d => d.FirstName + " " + d.LastName)
                .ToListAsync();

            receptionistDetails.AssignedDoctors = doctorNames;

            return View(receptionistDetails);
        }
    }
}
