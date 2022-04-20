using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
