using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Activities
{
    public class ActivityUserResponsesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ActivityUserResponsesModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ActivityUserResponse> ActivityUserResponses { get; set; }
        public Activity Activity { get; set; }
        public int CourseId { get; set; }

        public async Task<IActionResult> OnGet(int activityId, int courseId)
        {
            ActivityUserResponses = await _context.ActivityUserResponses
                .Include(aur => aur.User)
                .Include(aur => aur.Activity)
                .Where(aur => aur.ActivityId == activityId)
                .ToListAsync();
            Activity = await _context.Activities
                .FirstOrDefaultAsync(a => a.Id == activityId);
            CourseId = courseId;

            return Page();
        }
    }
}
