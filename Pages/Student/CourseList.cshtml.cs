using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Student
{
    public class CourseListModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CourseListModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Courses = await _context.Courses
                .Where(c => !c.Users.Contains(user))
                .ToListAsync();
        }

        public async Task<PartialViewResult> OnGetSearchResultAsync(string searchPhrase, string viewName)
        {
            searchPhrase = searchPhrase.ToUpper();
            Courses = await _context.Courses
                       .Where(c => c.Name.Contains(searchPhrase))
                       .ToListAsync();

            return Partial(viewName, Courses);
        }
    }
}
