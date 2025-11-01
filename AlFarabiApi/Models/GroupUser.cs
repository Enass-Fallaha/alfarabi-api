using System.ComponentModel.DataAnnotations;

namespace AlFarabiApi.Models
{
    public class GroupUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public double Cost { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User? User { get; set; }
        public Group? Group { get; set; }
    }
}
