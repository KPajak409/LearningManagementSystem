using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LMS.Pages.Admin
{
    public class IndexModel : PageModel
    {
        async public Task OnGet()
        {
            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            await Response.WriteAsJsonAsync(claims);
            
        }
    }
}
