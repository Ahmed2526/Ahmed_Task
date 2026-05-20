using Ahmed_Task.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ahmed_Task.Areas.Receptionist.ViewModel
{
    public class LocationVM
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public int GovernateId { get; set; }
        public int CityId { get; set; }

        public IEnumerable<SelectListItem>? Governate { get; set; }
        public IEnumerable<SelectListItem>? City { get; set; }

    }
}
