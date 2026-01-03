namespace AlFarabiApi.Models
{
    public class SubjectUser
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public  User? User { get; set; }
        public Subject? Subject { get; set; }


    }
}
