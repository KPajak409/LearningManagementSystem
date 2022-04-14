#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;

namespace LMS.Pages.Groups
{
    public class GroupListModel : PageModel
    {
        private readonly LMS.Data.ApplicationDbContext _context;

        public GroupListModel(LMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Group> Groups { get;set; }

        public async Task OnGetAsync()
        {
            Groups = await _context.Groups.ToListAsync();
        }
    }
}
