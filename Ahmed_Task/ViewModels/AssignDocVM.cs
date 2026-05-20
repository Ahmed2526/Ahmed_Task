using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ahmed_Task.ViewModels
{
    public class AssignDocVM
    {
        public string ReceptionistId { get; set; }

        public IEnumerable<SelectListItem> Doctors { get; set; }

        [Display(Name = "Doctor")] 
        public int SelectedDoctor { get; set; }

    }
}
