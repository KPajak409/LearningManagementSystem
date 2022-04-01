using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public int ActivityId { get; set; }
        public string Content { get; set; } = null!;
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public Activity Activity { get; set; } = null!;

        
    }
}
