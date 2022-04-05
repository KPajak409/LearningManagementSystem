#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using LMS.Dtos;
using AutoMapper;

namespace LMS.Pages.Teacher
{
    public class CreateCourseModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCourseModel(LMS.Data.ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreateCourseDto CreateCourseDto { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var courseEntity = _mapper.Map<Course>(CreateCourseDto);

            _context.Courses.Add(courseEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
