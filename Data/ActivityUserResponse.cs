using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class ActivityUserResponse
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        [Required]
        [MinLength(5)]
        public string Response { get; set; } = string.Empty;
        public string ResponseFileNames { get; set; } = string.Empty;
        public int? EarnedPoints { get; set; }
        public string Comment { get; set; } = null!;
        public ActivityStatus Status { get; set; }
    }
}
