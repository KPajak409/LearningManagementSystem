using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;

namespace LMS.Pages.Activities
{
    public class QuizDetailsModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;
        public QuizDetailsModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public Activity Activity { get; set; }
        public ActivityUserResponse UserResponse { get; set; }
        public DateTime DateTime { get; set; }
        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? activityId, int courseId)
        {
            if (activityId == null)
            {
                return NotFound();
            }

            Activity = await _context.Activities
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == activityId);
            Course = await _context.Courses
                .Include(c => c.Users)
                .SingleOrDefaultAsync(c => c.Id == courseId);
            var authorizationResult = _authorizationService.AuthorizeAsync(User, Course, new ResourceOperationRequirement(ResourceOperation.Read)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            var user = await _userManager.GetUserAsync(User);
            UserResponse = await _context.ActivityUserResponses
                .Include(aur => aur.Activity)
                .Where(aur => aur.ActivityId == activityId && aur.User.Id == user.Id)
                .SingleOrDefaultAsync();

            if (UserResponse == null)
                UserResponse = new ActivityUserResponse();
            DateTime = DateTime.Now;

            if (Activity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
