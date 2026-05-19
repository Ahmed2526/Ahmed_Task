namespace Ahmed_Task.ViewModels
{
    public class DoctorAvailablilityVM
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public TimeOnly AppointmentStart { get; set; }
        public TimeOnly AppointmentEnd { get; set; }
        public string Clinic { get; set; }
    }
}
