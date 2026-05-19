using Ahmed_Task.Models;

namespace Ahmed_Task.ViewModels
{
    public class ClinicVM
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }
    }
}
