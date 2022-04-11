using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace LMS.Pages.Users
{
    public class EditUserModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EditUserModel(LMS.Data.ApplicationDbContext context, UserManager<User> userManager)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByIdAsync(UserEntity.Id);
            user.Email = UserEntity.Email;
            user.FirstName = UserEntity.FirstName;
            user.LastName = UserEntity.LastName;
            await _userManager.UpdateAsync(user);

            _context.SaveChanges();

           
            return RedirectToPage("./UserDetails");
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
