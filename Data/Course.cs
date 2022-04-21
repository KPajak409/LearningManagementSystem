using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
namespace LMS.Data
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public virtual IList<Section> Sections { get; set; } = new List<Section>();
        public virtual IList<User> Users { get; set; } = new List<User>();
    }
}
