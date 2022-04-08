using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public int Position { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public virtual ICollection<Activity>? Activities { get; }
    }
}
