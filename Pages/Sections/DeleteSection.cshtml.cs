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
        [BindProperty]
        public string Error { get; set; }
        [BindProperty]
        public int CourseId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? sectionId, int courseId)
        {
            if (sectionId == null)
            {
                return NotFound();
            }

            Section = await _context.Sections
                .Include(s => s.Course).FirstOrDefaultAsync(m => m.Id == sectionId);
            CourseId = courseId;
            if (Section == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? sectionId)
        {
            if (sectionId == null)
            {
                return NotFound();
            }

            Section = await _context.Sections
                .Include(s => s.Activities)
                .FirstOrDefaultAsync(s => s.Id == sectionId);
            
            if (Section != null)
            {
                if(Section.Activities == null)
                {             
                    var sortedSections = _context.Sections
                    .Where(x => x.CourseId == Section.CourseId && x.Position > Section.Position)
                    .OrderBy(x => x.Position)
                    .ToList();

                    foreach (var section in sortedSections)
                        section.Position -= 1;
                    _context.Sections.Remove(Section);
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    Error = "You can't delete section, when there is atleast one activity. Delete all activities in section to proceed.";
                    return Page();
                }
            }

            return RedirectToPage("../Courses/CourseEditMode", new { id = Section.CourseId });
        }


    }
}
