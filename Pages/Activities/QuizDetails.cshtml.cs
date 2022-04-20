using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.AspNetCore.Identity;

namespace LMS.Pages.Activities
{
    public class QuizDetailsModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public QuizDetailsModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Activity Activity { get; set; }
        public ActivityUserResponse UserResponse { get; set; }
        public DateTime DateTime { get; set; }
        [BindProperty]
        public int CourseId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? activityId, int courseId)
        {
            if (activityId == null)
            {
                return NotFound();
            }

            Activity = await _context.Activities.FirstOrDefaultAsync(m => m.Id == activityId);
            var user = await _userManager.GetUserAsync(User);
            UserResponse = await _context.ActivityUserResponses
                .Include(aur => aur.Activity)
                .Where(aur => aur.ActivityId == activityId && aur.User.Id == user.Id)
                .SingleOrDefaultAsync();
            CourseId = courseId;
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
