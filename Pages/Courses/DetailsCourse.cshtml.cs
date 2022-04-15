#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;

namespace LMS.Pages.Courses
{
    public class DetailsCourseModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;

        public DetailsCourseModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }
        public IList<Section> Sections { get; set; }
        public async Task<IActionResult> OnGetAsync(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            Course = _context.Courses.Find(courseId);

            Sections = await _context.Sections
                .Where(x => x.CourseId == courseId)
                .OrderBy(x => x.Position)
                .Include(x => x.Activities).ToListAsync();

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
