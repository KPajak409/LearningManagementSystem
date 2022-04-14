using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LMS.Pages.Groups
{
    public class UserAssignationModel : PageModel
    {
        [BindProperty]
        public List<User> Users { get; set; }
        public Group Group { get; set; }
        public void OnGet()
        {
        }
        public void OnPost() { }
    }
}
