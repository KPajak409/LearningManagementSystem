using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace LMS.Pages.Student
{
    public class TaskListModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TaskListModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IList<Activity> Activities { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var userDb = await _context.Users
                .Include(c => c.Courses)
                .Where(u => u.Id == user.Id)
                .SingleOrDefaultAsync();

            Activities = await _context.Activities
                .Where(a => userDb.Courses.Contains(a.Course))
                .ToListAsync();
            
              

            Activities = Activities
                .Where(a => a.EndTime != null)
                .OrderByDescending(a => a.EndTime)
                .ThenBy(a => a.Name)
                .ToList();
            return Page();
        }
    }
}
