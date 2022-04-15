using AutoMapper;
using LMS.Data;
using LMS.Dtos;
using LMS.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Groups
{
    public class AssignUserToGroupModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AssignUserToGroupModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public UserAssignationModel UserAssign = new UserAssignationModel();
        [BindProperty]
        public Group Group { get; set; }
        [BindProperty]
        public IList<User> Users { get; set; } = new List<User>();
        public string ErrMsg { get; set; }
       // public UsersViewModel ViewModel { get; set; } = new UsersViewModel();
        public async Task<IActionResult> OnGet(int id)
        {
            Group = await _context.Groups
                .Include(g => g.Users)
                .Where(g => g.Id == id)
                .SingleOrDefaultAsync();
            Users = Group.Users;
            await OnGetSelectUsersInCurrentGroupAsync(id, "UsersList");
            return Page();
        }
        public async Task<PartialViewResult> OnGetSelectUsers(int id, string searchPhrase, string viewName)
        {
             searchPhrase = searchPhrase.ToUpper();
             UserAssign.Users = await _context.Users
                        .Where(u => u.FirstName.ToUpper().Contains(searchPhrase) || u.LastName.ToUpper().Contains(searchPhrase)).ToListAsync();
             UserAssign.Group = _context.Groups.Where(g => g.Id == id).SingleOrDefault();

             return Partial(viewName, UserAssign);

        }
        public async Task<PartialViewResult> OnGetSelectUsersInCurrentGroupAsync(int id, string viewName)
        {
            Group = await _context.Groups.Where(g => g.Id == id).SingleOrDefaultAsync();
            Users = await _context.Users
                        .Where(u => u.Groups.Contains(Group)).ToListAsync();
            return Partial(viewName, Users);
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var usersFromDb = new List<User>();
            Group = await _context.Groups
               .Include(u => u.Users)
               .Where(g => g.Id == id)
               .SingleOrDefaultAsync();
            var selectedUsers = Users.Where(u => u.IsSelected == true).ToList();
            foreach(var user in selectedUsers)
            {
                usersFromDb.Add(_context.Users.Where(u => u.Id == user.Id).SingleOrDefault());
            }
           
            if (Group == null )
                return NotFound();

            foreach (var user in usersFromDb)
            {
                if (!Group.Users.Contains(user) || !user.Groups.Contains(Group))
                {              
                    user.Groups.Add(Group);
                    Group.Users.Add(user);
                } else
                {
                    ErrMsg = "User:" + user.FirstName + user.LastName + "already in group.";
                    return Page();
                }
                    
            }
            await _context.SaveChangesAsync();
            Users = selectedUsers;
            await OnGet(id);
            return Page();
        }
    }
}
