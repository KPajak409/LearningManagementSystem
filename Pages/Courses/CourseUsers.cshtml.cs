using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Courses
{
    public class CourseUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CourseUsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }
        public int? TotalPoints { get; set; }
        public IList<UserPoints> Users { get; set; }

        public async Task<IActionResult> OnGet(int courseId)
        {
            Course = await _context.Courses
                .Include(c => c.Users)
                .Where(c => c.Id == courseId)
                .SingleOrDefaultAsync();
            var activities = await _context.Activities
                  .Where(x => x.CourseId == courseId)
                  .ToListAsync();
            TotalPoints = 0;

           

            foreach(var activity in activities)
            {
                if(activity.Points != null)
                    TotalPoints += activity.Points;
            }
            
            Users = new List<UserPoints>();
            foreach(var user in Course.Users)
            {
                var userActivities = await _context.ActivityUserResponses
                    .Where(a => a.User.Id == user.Id)
                    .ToListAsync();
                decimal? totalUserPoints = 0;
                foreach (var activity in userActivities)
                {
                    if(activity.EarnedPoints != null)
                        totalUserPoints += activity.EarnedPoints;
                }
                Users.Add(new UserPoints() { UserEntity = user, AchievedPoints = totalUserPoints });
            }
            return Page();
        }
    }
}
