using LMS.Data;

namespace LMS.Models
{
    public class UserPoints
    {
        public User UserEntity { get; set; }
        public decimal? AchievedPoints { get; set; }
    }
}
