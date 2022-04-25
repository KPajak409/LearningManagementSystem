using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime? StartTime { get; set; } 
        public DateTime? EndTime { get; set; } 
        public ActivityType ActivityType { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ActivityUserResponse> UserResponses { get; set; }
        public int? Points { get; set; } 
        public string FileNames { get; set; } 
     }
}
