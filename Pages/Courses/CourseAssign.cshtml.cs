using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Courses
{
    public class CourseAssignModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CourseAssignModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Course Course { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Error { get; set; }

        public async Task<IActionResult> OnGetAsync(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }

            Course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == courseId);

            if (Course == null)
            {
                return NotFound();
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Course = await _context.Courses
                .Include(c => c.Users)
                .Where(c => c.Id == Course.Id)
                .SingleOrDefaultAsync();
            var user = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(Course.PasswordHash))
            {
                Course.Users.Add(user);
            }
            else
            {
                if(BCrypt.Net.BCrypt.Verify(Password, Course.PasswordHash))
                {
                    Course.Users.Add(user);                  
                } else
                {
                    Error = "The password is incorrect";
                    return Page();
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("../Courses/DetailsCourse", new { courseId = Course.Id });
        }
    }
}
