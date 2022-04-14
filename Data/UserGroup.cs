namespace LMS.Data
{
    public class UserGroup
    {     
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
