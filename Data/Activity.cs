using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; } = null!;
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime? StartTime { get; set; } = null!;
        public DateTime? EndTime { get; set; } = null!;
        public ActivityType ActivityType { get; set; }
        public virtual IList<Question> Questions { get; set; }
        public virtual IList<ActivityUserResponse> UserResponses { get; set; }
        public int? Points { get; set; } = null!;
        public string FileNames { get; set; } = null!;
     }
}
