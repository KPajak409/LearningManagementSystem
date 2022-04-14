using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Groups
{
    public class DeassignUserFromGroupConfirmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeassignUserFromGroupConfirmModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Group Group { get; set; }
        [BindProperty]
        public User UserEntity { get; set; }
        public IActionResult OnGet(int groupId, string userId)
        {
            Group = _context.Groups
                .Include(g => g.Users)
                .FirstOrDefault(g => g.Id == groupId);
            UserEntity = Group.Users.Where(g => g.Id == userId).FirstOrDefault();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int groupId, string userId)
        {
            Group = await _context.Groups
                .Include(u => u.Users)
                .Where(g => g.Id == groupId).FirstOrDefaultAsync();
            UserEntity = await _context.Users
                .Include(g => g.Groups)
                .Where(u => u.Id == userId).FirstOrDefaultAsync();

            Group.Users.Remove(UserEntity);
            UserEntity.Groups.Remove(Group);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Groups/DeassignUserFromGroup", new { groupId, userId });
        }
    }
}
