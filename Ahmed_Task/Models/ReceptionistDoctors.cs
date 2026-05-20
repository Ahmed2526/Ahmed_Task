using System.ComponentModel.DataAnnotations.Schema;

namespace Ahmed_Task.Models
{
    public class ReceptionistDoctors
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }

        [ForeignKey(nameof(Receptionist))]
        public string ReceptionistId { get; set; }

        public ApplicationUser Receptionist { get; set; }
    }
}
