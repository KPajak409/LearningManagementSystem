using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Content { get; set; } = null!;
        public IList<Answer> Answers { get; set; }
        

        
    }
}
