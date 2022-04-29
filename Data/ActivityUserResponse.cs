using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class ActivityUserResponse
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public string Response { get; set; }
        public string ResponseFileNames { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EarnedPoints { get; set; }
        public string Comment { get; set; } = null!;
        public ActivityStatus Status { get; set; }
    }
}
