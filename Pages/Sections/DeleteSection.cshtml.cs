#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;

namespace LMS.Pages.Sections
{
    public class DeleteSectionModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;

        public DeleteSectionModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Section Section { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Section = await _context.Sections
                .Include(s => s.Course).FirstOrDefaultAsync(m => m.Id == id);

            if (Section == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Section = await _context.Sections.FindAsync(id);

            

            

            if (Section != null)
            {
                

                var sortedSections = _context.Sections
                .Where(x => x.CourseId == Section.CourseId && x.Position > Section.Position)
                .OrderBy(x => x.Position).ToList();

                foreach (var section in sortedSections)
                    section.Position -= 1;
                _context.Sections.Remove(Section);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Courses/CourseEditMode", new { id = Section.CourseId });
        }

        private static int first_available_number(List<Section> sections)
        {

            return 1;
        }
    }
}
