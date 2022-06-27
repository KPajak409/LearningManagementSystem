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
        [BindProperty]
        public int CourseId { get; set; }
        public QuizUserStartModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        

        public async Task<IActionResult> OnGetAsync(int activityId, int courseId)
        {
            CourseId = courseId;
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

            var user = await _userManager.GetUserAsync(User);
            var userResponse = await _context.ActivityUserResponses
                .Include(x => x.Activity)
                .Where(x => x.User.Id == user.Id)
                .FirstOrDefaultAsync();
            if(userResponse == null)
                return Page();

            return RedirectToPage("QuizDetails", new { activityId = activityId});
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userResponse = new ActivityUserResponse();
            userResponse.User = user;
            userResponse.ActivityId = ActivityId;
            userResponse.Status = ActivityStatus.QuizEnded;
            userResponse.EarnedPoints = 0;
            var dbQuestions = await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.Activity)
                .Where(q => q.ActivityId == ActivityId)
                .ToListAsync();
            decimal pointsTmp;
            int numberOfCorrectAnswers;
            for(int i = 0; i < Questions.Count;i++)
            {
                pointsTmp = 0;
                numberOfCorrectAnswers = dbQuestions[i].Answers.Count(a => a.IsCorrect);
                for (int j = 0; j < Questions[i].Answers.Count;j++)
                {
                    if (numberOfCorrectAnswers > 1)
                    {
                                               
                            if (Questions[i].Answers[j].IsSelected == dbQuestions[i].Answers[j].IsCorrect && dbQuestions[i].Answers[j].IsCorrect == true)
                            {
                                pointsTmp += (1.0m / numberOfCorrectAnswers);
                            } else if (Questions[i].Answers[j].IsSelected == true &&  dbQuestions[i].Answers[j].IsCorrect == false)
                            {
                                pointsTmp = 0;
                                break;
                            }
                    } 
                    else
                    {
                        if (dbQuestions[i].Answers[j].IsCorrect)
                            if (Questions[i].Answers[j].IsSelected == dbQuestions[i].Answers[j].IsCorrect)
                                userResponse.EarnedPoints += 1;
                    }
                }
                userResponse.EarnedPoints += pointsTmp;

            }
            _context.ActivityUserResponses.Add(userResponse);
            await _context.SaveChangesAsync();
            return RedirectToPage("QuizDetails", new { activityId = ActivityId, courseId = CourseId});
        }
    }
}
