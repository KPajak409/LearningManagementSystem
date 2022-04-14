#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
#nullable disable
namespace LMS.Pages.Groups
{
    public class GroupDetailsModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;

        public GroupDetailsModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Group Group { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Group == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
