namespace Ahmed_Task.ViewModels
{
    public class PatientVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
