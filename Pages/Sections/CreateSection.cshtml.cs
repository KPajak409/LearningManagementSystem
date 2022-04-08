#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;

namespace LMS.Pages.Sections
{
    public class CreateSectionModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;

        public CreateSectionModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Section Section { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Sections.Add(Section);
            await _context.SaveChangesAsync();
            return RedirectToPage("../Courses/CourseEditMode", new {id = Section.CourseId});
        }
    }
}
