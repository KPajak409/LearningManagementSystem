using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;

namespace LMS.Pages.Activities
{
    public class UserResponseDetailsModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserResponseDetailsModel(LMS.Data.ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [BindProperty]
        public ActivityUserResponse ActivityUserResponse { get; set; }
        public Activity Activity { get; set; }
        [BindProperty]
        public IList<FileModel> DownloadedFiles { get; set; }

        public async Task<IActionResult> OnGetAsync(int? responseId)
        {
            if (responseId == null)
            {
                return NotFound();
            }

            ActivityUserResponse = await _context.ActivityUserResponses
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(m => m.Id == responseId);
            Activity = ActivityUserResponse.Activity;

            if (ActivityUserResponse == null)
            {
                return NotFound();
            }
            return Page();
        }
        public FileResult OnGetDownloadResponseFile(string fileName)
        {
            string path = Path.Combine(_environment.ContentRootPath, "Files\\UserResponses\\" + $"{ActivityUserResponse.Id}\\") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Activity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == ActivityUserResponse.ActivityId);
            if(ActivityUserResponse.EarnedPoints > Activity.Points)
            {
                return BadRequest();
            }
            ActivityUserResponse.Status = ActivityStatus.Assessed;
            _context.Attach(ActivityUserResponse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityUserResponseExists(ActivityUserResponse.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./ActivityUserResponses", new { activityId = ActivityUserResponse.ActivityId });
        }

        private bool ActivityUserResponseExists(int id)
        {
            return _context.ActivityUserResponses.Any(e => e.Id == id);
        }
    }
    
}
