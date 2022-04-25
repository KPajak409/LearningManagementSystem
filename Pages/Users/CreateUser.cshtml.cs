using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
#nullable disable
namespace LMS.Pages.Users
{
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private static Random random = new Random();
        

        public CreateUserModel(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Email Email { get; set; } = new Email();
        public List<SelectListItem> SelectListItems { get; set; }
        [BindProperty]
        public User UserEntity { get; set; }
        [BindProperty]
        public string SelectedRole { get; set; }
        public string Error { get; set; }

        public IActionResult OnGet()
        {
            SelectListItems = _context.Roles.Select(i => new SelectListItem
            {
                Value = i.Name.ToString(),
                Text = i.Name,
            }).ToList();
            
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
                
            if(_userManager.FindByEmailAsync(UserEntity.Email).Result == null)
            {
                var generatedPassword = GeneratePassword();

                MailMessage mailMessage = new MailMessage();
                Email.To = UserEntity.Email;
                Email.Subject = "The account has been created for you";
                Email.Body = "Your account is ready to use with the following login credentials:\n" +
                    $"Login: {Email.To}\nPassword:{generatedPassword}";
                    
                mailMessage.To.Add(Email.To);
                mailMessage.Subject = Email.Subject;
                mailMessage.Body = Email.Body;
                mailMessage.From = new MailAddress("1c58350fad-0eabcd@inbox.mailtrap.io");
                SmtpClient smtp = new SmtpClient("smtp.mailtrap.io", 2525);
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("67ec45cbbd0834", "1053a53f7a1331");
                await smtp.SendMailAsync(mailMessage);
                ViewData["Message"] = "The mail with login credentials has been sent to " + Email.To;

                UserEntity.UserName = UserEntity.Email;
                IdentityResult result = await _userManager.CreateAsync(UserEntity, generatedPassword);
                
                if (result.Succeeded)
                    _userManager.AddToRoleAsync(UserEntity, SelectedRole).Wait();
                await _context.SaveChangesAsync();
            } else
            {
                Error = "User with the given adress email already exists.";
            }
            OnGet(); 
            return Page();
        }
        public static string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var password = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return password += "a@"; ;
        }
    }
}
