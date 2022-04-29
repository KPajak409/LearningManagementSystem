#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS;
using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LMS.Pages.Courses
{
    public class DeleteCourseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;

        public DeleteCourseModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<User> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;

        }

        [BindProperty]
        public Course Course { get; set; }
        

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses.FindAsync(id);
            var authorizationResult = _authorizationService.AuthorizeAsync(User, Course, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (Course != null)
            {
                _context.Courses.Remove(Course);
                await _context.SaveChangesAsync();
            }
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Teacher"))
                return RedirectToPage("../Teacher/Index");
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToPage("../Admin/Index");

            return RedirectToPage("./CourseList");
        }
    }
}
