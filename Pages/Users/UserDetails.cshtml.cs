using AutoMapper;
using LMS.Data;
using LMS.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
#pragma warning disable 8618
namespace LMS.Pages.Users
{
    public class UserDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserDetailsModel(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IList<UserDto> UsersDto { get; set; }

        public async Task OnGetAsync()
        {
            var users = await _context.Users
                .Include(g => g.Groups).ToListAsync();
            UsersDto = _mapper.Map<List<UserDto>>(users);
        }
    }
}
