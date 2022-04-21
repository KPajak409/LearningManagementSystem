using LMS.Data;

namespace LMS.Models
{
    public class UserPoints
    {
        public User UserEntity { get; set; }
        public int? AchievedPoints { get; set; }
    }
}
