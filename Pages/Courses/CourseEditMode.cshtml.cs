#nullable disable
using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Courses
{
    public class CourseEditModeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CourseEditModeModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Course Course { get; set; }
        public IList<Section> Sections { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course = _context.Courses.Find(id);
                
            Sections = await _context.Sections
                .Include(x => x.Activities).ToListAsync();
               
            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }


    }
}
