#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using System.ComponentModel.DataAnnotations;

namespace LMS.Pages.Activities
{
    public class CreateActivityModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private IWebHostEnvironment _environment;

        [BindProperty]
        public Activity Activity { get; set; }
        [EnumDataType(typeof(ActivityType))]
        public ActivityType SelectedActivityType { get; set; }
        [BindProperty]
        public IList<IFormFile> Files { get; set; } = new List<IFormFile>();
        [BindProperty]
        public string FileName { get; set; } = string.Empty;
        public CreateActivityModel(LMS.Data.ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int courseId, int sectionId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(Activity);
            _context.SaveChanges();

            //var createdActivity = _context.Activities.Find(a => a.Id)
            var filePaths = new List<string>();
            var size = Files.Sum(f => f.Length);
            foreach(var formFile in Files)
            {

                if(formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\", formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            

            return RedirectToPage("../Courses/CourseEditMode", new  { id = courseId});
        }
    }
}
