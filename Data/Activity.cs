using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ActivityType ActivityType { get; set; }
        public ICollection<Question>? Questions { get; set; }
        public int? Points { get; set; }

     }
}
