using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        [Required]
        public string Content { get; set; } = null!;
        public virtual IList<Answer> Answers { get; set; } = null!;
        

        
    }
}
