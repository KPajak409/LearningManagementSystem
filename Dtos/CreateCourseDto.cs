using System.ComponentModel.DataAnnotations;

namespace LMS.Dtos
{
    public class CreateCourseDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
