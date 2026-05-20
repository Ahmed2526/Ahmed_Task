using Microsoft.AspNetCore.Identity;
namespace Ahmed_Task.Models;
public class ApplicationUser : IdentityUser
{
    public IEnumerable< ReceptionistDoctors> ReceptionistDoctors { get; set; }
}
