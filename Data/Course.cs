using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string AuthorId { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<User> Users { get; } = new List<User>();      
    }
}
