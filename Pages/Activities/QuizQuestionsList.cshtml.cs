using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Activities
{
    public class QuizQuestionsListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Activity Activity { get; set; }
        public IList<Question> Questions { get; set; }
        [BindProperty]
        public int NumberOfAnswers { get; set; }
        public QuestionType QuestionType { get; set; }
        public QuizQuestionsListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int activityId)
        {
            
            Questions = await _context.Questions
                .Include(q => q.Activity)
                .Include(q => q.Answers)
                .Where(q => q.ActivityId == activityId)
                .ToListAsync();
            Activity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == activityId);
            if(Questions is null)
            {
                Questions=new List<Question>();
            }
            return Page();
        }
    }
}
