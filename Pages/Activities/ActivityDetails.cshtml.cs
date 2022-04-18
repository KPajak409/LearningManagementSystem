using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LMS.Models;
using System.IO;
namespace LMS.Pages.Activities
{
    public class ActivityDetailsModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public Activity Activity { get; set; }
        [BindProperty]
        public IList<IFormFile> Files { get; set; } = new List<IFormFile>();
        [BindProperty]
        public ActivityUserResponse UserResponse { get; set; }
        [BindProperty]
        public IList<FileModel> DownloadedFiles { get; set; }
        [BindProperty]
        public int CourseId { get; set; }

        public ActivityDetailsModel(LMS.Data.ApplicationDbContext context, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public IActionResult OnGet(int activityId, int courseId)
        {
            Activity =  _context.Activities
               .FirstOrDefault(m => m.Id == activityId);
            UserResponse =  _context.ActivityUserResponses
                .FirstOrDefault(ur => ur.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value && ur.ActivityId == activityId);
            CourseId = courseId;

            if (Activity == null)
            {
                return NotFound();
            }
            if (UserResponse == null)
            {
                UserResponse = new ActivityUserResponse();
            }

            DownloadedFiles = new List<FileModel>();
            var sourceFilePaths = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Files\\Activities\\" + $"{Activity.Id}\\"));  
          
            
            foreach (string filePath in sourceFilePaths)
            {
                var fileName = Path.GetFileName(filePath);
                if (string.IsNullOrEmpty(Activity.FileNames))
                {
                    break;
                }
                else if (Activity.FileNames.Contains(fileName))
                {
                    DownloadedFiles.Add(new FileModel
                    {
                        FileName = fileName,
                        Owner = OwnerType.Teacher
                    });
                }                       
            }
            if (UserResponse.Id != 0)
            {
                var responseFilePaths = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Files\\UserResponses\\" + $"{UserResponse.Id}\\"));
                foreach (string filePath in responseFilePaths)
                {
                    var fileName = Path.GetFileName(filePath);
                    if (string.IsNullOrEmpty(UserResponse.ResponseFileNames))
                    {
                        break;
                    }
                    else if (UserResponse.ResponseFileNames.Contains(fileName))
                    {
                        DownloadedFiles.Add(new FileModel
                        {
                            FileName = fileName,
                            Owner = OwnerType.Student
                        });
                    }
                }
            }

            

            return Page();
        }

        public FileResult OnGetDownloadActivityFile(string fileName)
        {
            string path = Path.Combine(_environment.ContentRootPath, "Files\\Activities\\" + $"{Activity.Id}\\") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        public FileResult OnGetDownloadResponseFile(string fileName)
        {
            string path = Path.Combine(_environment.ContentRootPath, "Files\\UserResponses\\" + $"{UserResponse.Id}\\") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (Files.Count > 0)
            {               
                var filePaths = new List<string>();
                var size = Files.Sum(f => f.Length);
                foreach (var formFile in Files)
                {

                    if (formFile.Length > 0)
                    {
                        if (Files.Last() != formFile)
                        {
                            if (UserResponse.ResponseFileNames == null)
                            {
                                UserResponse.ResponseFileNames = string.Empty;
                                UserResponse.ResponseFileNames += formFile.FileName + ' ';
                            }
                            else
                            {
                                UserResponse.ResponseFileNames += formFile.FileName + ' ';
                            }
                        }
                        else
                        {
                            if (UserResponse.ResponseFileNames == null)
                            {
                                UserResponse.ResponseFileNames = string.Empty;
                                UserResponse.ResponseFileNames += formFile.FileName;
                            }
                            else
                            {
                                UserResponse.ResponseFileNames += formFile.FileName;
                            }
                        }                   
                    }
                    else if (formFile.Length == 0)
                    {
                        UserResponse.ResponseFileNames = string.Empty;
                    }
                }
            }
            Activity = await _context.Activities
                .Include(a => a.UserResponses)
                .Where(a => a.Id == Activity.Id)
                .FirstOrDefaultAsync();
            if(Activity.UserResponses == null)
            {
                Activity.UserResponses = new List<ActivityUserResponse>();
            }

            UserResponse.ActivityId = Activity.Id;
            UserResponse.User = await _userManager.GetUserAsync(User);
            Activity.UserResponses.Add(UserResponse);
            await _context.SaveChangesAsync();

            if(Files.Count > 0)
            {
                var filePaths = new List<string>();
                var folderPath = Path.Combine(_environment.ContentRootPath, "Files\\UserResponses\\" + $"{UserResponse.Id}\\");
                Directory.CreateDirectory(folderPath);
                foreach(var formFile in Files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\UserResponses\\" + $"{UserResponse.Id}\\", formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
                
            }
            

            return Page();
        }
    }
}
