using Ahmed_Task.Data;
using Ahmed_Task.Enums;
using Ahmed_Task.Models;
using Ahmed_Task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ahmed_Task.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class SpecialitiesController : Controller
    {
        private readonly MedLinkDbContext _context;
        public SpecialitiesController(MedLinkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var specialities = await _context.Specialities.Select(s => new SpecialityVM
            {
                Id = s.Id,
                Name = s.Name
            }).ToListAsync();

            return View(specialities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialityFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var speciality = new Speciality
            {
                Name = model.Name
            };

            _context.Specialities.Add(speciality);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var speciality = await _context.Specialities.FindAsync(id);

            if (speciality == null)
                return NotFound();

            var model = new SpecialityFormVM
            {
                Name = speciality.Name
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialityFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var speciality = await _context.Specialities.FindAsync(id);

            if (speciality == null)
                return NotFound();

            speciality.Name = model.Name;

            _context.Specialities.Update(speciality);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var speciality = await _context.Specialities.FindAsync(id);

            if (speciality == null)
                return NotFound();

            _context.Specialities.Remove(speciality);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
