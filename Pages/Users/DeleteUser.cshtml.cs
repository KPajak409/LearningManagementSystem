using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#nullable disable
namespace LMS.Pages.Users
{
    public class DeleteUserModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DeleteUserModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public User UserEntity { get; set; }
        public string Error { get; set; }
        public IActionResult OnGetAsync(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            UserEntity = _context.Users.Where(u => u.Id == userId).FirstOrDefault();           

            if (UserEntity == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            UserEntity = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            if(UserEntity == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(UserEntity);
            if(result.Succeeded)
            { 
                return RedirectToPage("../Users/UserDetails", new { id = userId }); 
            }
            Error = "Something went wrong while deleting user.";
            return Page();     
        }
    }
}
