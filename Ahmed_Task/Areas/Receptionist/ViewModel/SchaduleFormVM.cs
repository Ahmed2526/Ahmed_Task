using Ahmed_Task.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ahmed_Task.Areas.Receptionist.ViewModel
{
    public class SchaduleFormVM
    {
        public int? Id { get; set; }
        public string Day { get; set; }
        public TimeOnly AppointmentStart { get; set; }
        public TimeOnly AppointmentEnd { get; set; }
        public int DoctorId { get; set; }

        public int ClinicId { get; set; }
        public List<SelectListItem>? Clinics { get; set; }
    }
}
