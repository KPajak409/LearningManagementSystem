using System.ComponentModel.DataAnnotations;

namespace LMS.Data
{
    public class StudentGrade
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        
        public virtual User Student { get; set; }
        public virtual Course Course { get; set; }

        public StudentGrade(User student, Course course)
        {
            Student = student;
            StudentId = student.Id;
            Course = course;
            CourseId = course.Id;

        }
    }
}
