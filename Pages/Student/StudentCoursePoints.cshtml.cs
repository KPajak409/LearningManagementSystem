using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Student
{
    public class UserCoursePointsDetailsModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserCoursePointsDetailsModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IList<UserActivityResponse> ActivitiesWithResponse { get; set; }
        public User UserEntity { get; set; }

        public async Task<IActionResult> OnGet(string studentId, int courseId)
        {
            ActivitiesWithResponse = new List<UserActivityResponse>();
            
            UserEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            var activities = await _context.Activities
                   .Include(a => a.Course)
                   .Include(a => a.UserResponses)
                   .Where(a => a.Course.Id == courseId)
                   .ToListAsync();
            var userResponses = await _context.ActivityUserResponses
                .Include(ur => ur.Activity)
                .Include(ur => ur.User)
                .Where(ur => ur.User.Id == studentId && ur.Activity.Course.Id == courseId)
                .ToListAsync();
            if (!userResponses.Any())
            {
                for (int i = 0; i < activities.Count; i++)
                    if (activities[i].ActivityType != ActivityType.Information)
                        ActivitiesWithResponse.Add(new UserActivityResponse() { Activity = activities[i] });
                return Page();
            }


            for (int i = 0; i < activities.Count;i++)
            {
                if (activities[i].ActivityType != ActivityType.Information)
                {
                    for (int j = 0; j < userResponses.Count; j++)
                    {

                        if (activities[i].Id == userResponses[j].ActivityId)
                        {
                            ActivitiesWithResponse.Add(new UserActivityResponse() { Activity = activities[i], UserResponse = userResponses[j] });
                            break;
                        }
                        if (!activities[i].UserResponses.Contains(userResponses[j]))
                        {
                            ActivitiesWithResponse.Add(new UserActivityResponse() { Activity = activities[i] });
                            break;
                        }
                    }

                }

            }
           
            return Page();
        }
    }
}
