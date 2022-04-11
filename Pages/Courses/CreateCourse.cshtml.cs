#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using LMS.Dtos;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace LMS.Pages.Courses
{
    public class CreateCourseModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CreateCourseModel(LMS.Data.ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreateCourseDto CourseDto { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var courseEntity = _mapper.Map<Course>(CourseDto);
            var user = await _userManager.GetUserAsync(User);
            courseEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(CourseDto.Password);
            courseEntity.Author = user;
            courseEntity.AuthorId = user.Id;
            _context.Courses.Add(courseEntity);
            await _context.SaveChangesAsync();

            if (await _userManager.IsInRoleAsync(user, "Teacher"))
                return RedirectToPage("../Teacher/Index");
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToPage("../Admin/Index");

            return Page();
        }
    }
}
