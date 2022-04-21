using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.NewFolder
{
    public class IndexModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public IList<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Courses = await _context.Courses
                .Where(c => c.Users.Contains(user))
                .ToListAsync();
        }
    }
}
