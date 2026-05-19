using Ahmed_Task.Models;

namespace Ahmed_Task.ViewModels
{
    public class AppointmentVM
    {
        public int Id { get; set; }

        public string? Patient { get; set; }
        public string? Doctor { get; set; }

        public string Clinic { get; set; }

        public TimeOnly AppointmentStart { get; set; }

        public TimeOnly AppointmentEnd { get; set; }

        public string Status { get; set; }

        public DateOnly Day { get; set; }
    }
}
