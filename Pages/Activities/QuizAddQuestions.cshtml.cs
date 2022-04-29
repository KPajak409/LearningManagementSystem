using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Activities
{
    public class QuizAddQuestionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Activity Activity { get; set; }
        [BindProperty]
        public Question Question { get; set; }
        [BindProperty]
        public IList<Answer> Answers { get; set; }
        public QuizAddQuestionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public  IActionResult OnGet(int activityId, int numberOfAnswers, QuestionType questionType)
        {
            Activity = _context.Activities.SingleOrDefault(a => a.Id == activityId);

            Question = new Question();
            Answers = new List<Answer>();
            Question.QuestionType = (QuestionType)questionType;

            for(int i =0; i < numberOfAnswers; i++)
            {
                Answers.Add(new Answer());
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Activity = await _context.Activities.SingleOrDefaultAsync(a => a.Id == Activity.Id);
            if(Activity.Points == null)
            {
                Activity.Points = 1;
            }
            else 
            { 
                Activity.Points +=  1; 
            }
            Question.Activity = Activity;
            Question.Answers = Answers;
            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();
            return RedirectToPage("QuizQuestionsList", new { activityId = Activity.Id});
        }
    }
}
