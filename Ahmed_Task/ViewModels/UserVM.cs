namespace Ahmed_Task.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLockedOut { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
