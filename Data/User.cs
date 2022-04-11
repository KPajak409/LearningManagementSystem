using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class User : IdentityUser
    {
        [Required/*(ErrorMessage = "The field email is required")*/]
        [EmailAddress/*(ErrorMessage = "Inappriopriate email adress")*/]
        public override string Email { get; set; } = string.Empty;
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
    }
}
