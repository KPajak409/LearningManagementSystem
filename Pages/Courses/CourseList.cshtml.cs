using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Courses
{
    public class CourseListModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;

        public CourseListModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Author).ToListAsync();
        }
    }
}
