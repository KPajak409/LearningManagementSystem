using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Position { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public virtual Course Course { get; set; } = new Course();
        public virtual ICollection<Activity> Activities { get; } = new List<Activity>();
    }
}
