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
using Microsoft.AspNetCore.Authorization;
using LMS.Authorization;

namespace LMS.Pages.Activities
{
    public class DetailsActivityModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;
        [BindProperty]
        public Activity Activity { get; set; }
        [BindProperty]
        public IList<IFormFile> Files { get; set; } = new List<IFormFile>();
        [BindProperty]
        public ActivityUserResponse UserResponse { get; set; }
        [BindProperty]
        public IList<FileModel> DownloadedFiles { get; set; }
        [BindProperty]
        public Course Course { get; set; }

        public DetailsActivityModel(LMS.Data.ApplicationDbContext context, IWebHostEnvironment environment, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public IActionResult OnGet(int activityId, int courseId)
        {
            Activity =  _context.Activities
               .Include(c => c.Course)
               .FirstOrDefault(m => m.Id == activityId);
            Course = _context.Courses
                .Include(c => c.Users)
                .SingleOrDefault(c => c.Id == courseId);

            var authorizationResult = _authorizationService.AuthorizeAsync(User, Course, new ResourceOperationRequirement(ResourceOperation.Read)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            UserResponse =  _context.ActivityUserResponses
                .FirstOrDefault(ur => ur.User.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value && ur.ActivityId == activityId);


            if (Activity == null)
            {
                return NotFound();
            }
            if (UserResponse == null)
            {
                UserResponse = new ActivityUserResponse();
            }

            DownloadedFiles = new List<FileModel>();

            string curDir = Path.Combine(_environment.ContentRootPath, "Files\\Activities\\" + $"{Activity.Id}\\");
            if (Directory.Exists(curDir))
            {
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
            }
            curDir = Path.Combine(_environment.ContentRootPath, "Files\\UserResponses\\" + $"{UserResponse.Id}\\");
            if (Directory.Exists(curDir))
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
                .Include(c => c.Course)
                .Include(a => a.UserResponses)
                .Where(a => a.Id == Activity.Id)
                .FirstOrDefaultAsync();

            var authorizationResult = _authorizationService.AuthorizeAsync(User, Activity.Course, new ResourceOperationRequirement(ResourceOperation.Respond)).Result;
            if (!authorizationResult.Succeeded)
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (Activity.UserResponses == null)
            {
                Activity.UserResponses = new List<ActivityUserResponse>();
            }

            UserResponse.ActivityId = Activity.Id;
            UserResponse.User = await _userManager.GetUserAsync(User);
            UserResponse.Status = ActivityStatus.Assessed;
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
