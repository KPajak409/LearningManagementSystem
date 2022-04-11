using LMS.Data;

namespace LMS.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public IList<Group> Groups { get; set; } = new List<Group>();
    }
}
