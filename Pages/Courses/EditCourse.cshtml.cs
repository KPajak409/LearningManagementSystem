#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;

namespace LMS.Pages.Courses
{
    public class EditCourseModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        public EditCourseModel(LMS.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Course Course { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
            {
                return NotFound();
            }
           ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(User, Course, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (!string.IsNullOrEmpty(Password))
            {
                Course.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
            }
            _context.Attach(Course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Courses/CourseList");
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
