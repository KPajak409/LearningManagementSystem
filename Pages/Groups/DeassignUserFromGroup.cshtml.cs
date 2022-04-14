using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Groups
{
    public class DeassignUserFromGroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeassignUserFromGroupModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Group Group { get; set; }
        [BindProperty]
        public User UserEntity { get; set; }
        public IActionResult OnGet(int groupId)
        {
            Group = _context.Groups
                .Include(g => g.Users)
                .FirstOrDefault(g => g.Id == groupId);
            
            return Page();
        }
    }
}
