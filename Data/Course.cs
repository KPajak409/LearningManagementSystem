using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace LMS.Data
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string AuthorId { get; set; } = null!;
        public virtual User Author { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }    
    }
}
