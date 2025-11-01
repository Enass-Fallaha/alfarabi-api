//using AlFarabiApi.Models;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Models
{
    public class Group
    { 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
     
     
        public GenderEnum Gender { get; set; }

        public List<GroupUser>? GroupUsers { get; set; }

        public int LevelId { get; set; }
        public Level? Level { get; set; }
    }
}
