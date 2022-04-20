using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging;

namespace LMS.Pages.Activities
{
    public class QuizUserStartModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public int ActivityId { get; set; }
        [BindProperty]
        public IList<Question> Questions { get; set; }
        [BindProperty]
        public IList<Answer> Answers { get; set; }

        public QuizUserStartModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        

        public async Task<IActionResult> OnGetAsync(int activityId)
        {
            Questions = await _context.Questions
                .Include(q => q.Activity)
                .Include(q => q.Answers)
                .Where(q => q.ActivityId == activityId)
                .ToListAsync();
            Answers = new List<Answer>();
            foreach(var question in Questions)
            {
                Answers.AddRange(question.Answers);
            }
            var userResponse = await _context.ActivityUserResponses
                .Include(x => x.Activity)
                .Where(x => x.ActivityId == activityId)
                .FirstOrDefaultAsync();
            if(userResponse == null)
                return Page();

            return RedirectToPage("QuizDetails", new { activityId = activityId});
        }

        public async Task<IActionResult> OnPostAsync(List<Answer> answers)
        {
            var user = await _userManager.GetUserAsync(User);
            var userResponse = new ActivityUserResponse();
            userResponse.User = user;
            userResponse.ActivityId = ActivityId;
            userResponse.Status = ActivityStatus.QuizEnded;
            userResponse.EarnedPoints = 0;
            var dbAnswers = await _context.Answers
                .Where(a => answers.Contains(a))
                .ToListAsync();
            for(var i = 0; i < answers.Count; i++)
            {
                if(dbAnswers[i].IsCorrect)
                    userResponse.EarnedPoints += 1;
            }
            _context.ActivityUserResponses.Add(userResponse);
            await _context.SaveChangesAsync();
            return RedirectToPage("QuizDetails", new { activityId = ActivityId});
        }
    }
}
