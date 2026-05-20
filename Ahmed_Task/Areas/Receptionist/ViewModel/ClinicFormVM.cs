using Ahmed_Task.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ahmed_Task.Areas.Receptionist.ViewModel
{
    public class ClinicFormVM
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Price { get; set; }
        public int DoctorId { get; set; }
        public int LocationId { get; set; }
        public int SpecialityId { get; set; }

        public LocationVM Location { get; set; }
        public IEnumerable<SelectListItem>? Speciality { get; set; }
    }
}
