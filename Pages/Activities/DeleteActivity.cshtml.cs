using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;

namespace LMS.Pages.Activities
{
    public class DeleteActivityModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        public DeleteActivityModel(LMS.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Activity Activity { get; set; }
        [BindProperty]
        public int CourseId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? activityId, int courseId)
        {
            if (activityId == null)
            {
                return NotFound();
            }

            Activity = await _context.Activities.FirstOrDefaultAsync(m => m.Id == activityId);
            CourseId = courseId;
            if (Activity == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            Activity = await _context.Activities.FindAsync(Activity.Id);

            var authorizationResult = _authorizationService.AuthorizeAsync(User, Activity.Course, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (Activity != null)
            {
                _context.Activities.Remove(Activity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Courses/CourseEditMode", new { id = CourseId });
        }
    }
}
