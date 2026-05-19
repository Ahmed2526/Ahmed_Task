using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ahmed_Task.ViewModels
{
    public class UserFormVM
    {
        public string Id { get; set; }
        public List<SelectListItem>? Roles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
