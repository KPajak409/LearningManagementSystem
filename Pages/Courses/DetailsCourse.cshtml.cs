#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;

namespace LMS.Pages.Courses
{
    public class DetailsCourseModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DetailsCourseModel(LMS.Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public Course Course { get; set; }
        public IList<Section> Sections { get; set; }

        public async Task<IActionResult> OnGetAsync(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            Course = _context.Courses.Find(courseId);
               
            var courseWithUsers =  await _context.Courses
                .Include(c => c.Users).SingleOrDefaultAsync(c => c.Id == courseId);
            var authorizationResult = _authorizationService.AuthorizeAsync(User, courseWithUsers, new ResourceOperationRequirement(ResourceOperation.Read)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            Sections = await _context.Sections
                .Where(x => x.CourseId == courseId)
                .OrderBy(x => x.Position)
                .Include(x => x.Activities).ToListAsync();
  
            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
