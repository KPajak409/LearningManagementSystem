namespace LMS.Models
{
    public enum OwnerType
    {
        Student,
        Teacher
    }
    public class FileModel
    {
        public string FileName { get; set; }
        public OwnerType Owner { get; set; }
    }
}
