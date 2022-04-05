namespace LMS.Dtos
{
    public class CreateCourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        private string? Password { get; set; }
    }
}
