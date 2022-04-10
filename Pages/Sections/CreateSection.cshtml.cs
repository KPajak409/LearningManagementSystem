#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Sections
{
    public class SectionCreateModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        
        public SectionCreateModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int ? courseId)
        {
            ViewData["CourseId"] = courseId;
            return Page();
        }

        [BindProperty]
        public Section Section { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? courseId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var sectionsInCourse = _context.Sections
                .Where(x => x.CourseId == courseId)
                .OrderBy(x => x.Position).ToList();
            if(sectionsInCourse.Count ==0)
            {
                Section.Position = 1;
            } else
            {
                var lastPosition = sectionsInCourse.Last().Position;
                Section.Position = lastPosition + 1;
            }
           
            _context.Sections.Add(Section);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Courses/CourseEditMode", new { Id = courseId});
        }
    }
}
