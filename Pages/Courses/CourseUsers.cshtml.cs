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
            Course.Sections = await _context.Sections
                  .Where(x => x.CourseId == courseId)
                  .OrderBy(x => x.Position)
                  .Include(x => x.Activities)
                  .ToListAsync();
            TotalPoints = 0;

           
            foreach(var section in Course.Sections)
            {
                foreach(var activity in section.Activities)
                {
                    TotalPoints += activity.Points;
                }
            }
            Users = new List<UserPoints>();
            foreach(var user in Course.Users)
            {
                var userActivities = await _context.ActivityUserResponses
                    .Where(a => a.User.Id == user.Id)
                    .ToListAsync();
                int? totalUserPoints = 0;
                foreach (var activity in userActivities)
                {
                    totalUserPoints += activity.EarnedPoints;
                }
                Users.Add(new UserPoints() { UserEntity = user, AchievedPoints = totalUserPoints });
            }
            return Page();
        }
    }
}
