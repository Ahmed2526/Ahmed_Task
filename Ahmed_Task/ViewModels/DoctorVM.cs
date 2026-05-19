namespace Ahmed_Task.ViewModels
{
    public class DoctorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Speciality{ get; set; }

        public bool IsLockedOut { get; set; }
        public IEnumerable<ClinicVM>? Clinics { get; set; }
    }
}
