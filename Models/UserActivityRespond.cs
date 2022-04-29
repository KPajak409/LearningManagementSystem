using LMS.Data;

namespace LMS.Models
{
    public class UserActivityResponse
    {
        public Activity Activity { get; set; }
        public ActivityUserResponse UserResponse { get; set; }
    }
}
