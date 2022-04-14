using System.ComponentModel.DataAnnotations;
#pragma warning disable 8618
namespace LMS.Data
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; } = new List<User>();
    }
}
