using System.ComponentModel.DataAnnotations;

namespace Ahmed_Task.ViewModels
{
    public class SpecialityFormVM
    {
        [Required]
        [Length(5,150)]
        public string Name { get; set; }

    }
}
