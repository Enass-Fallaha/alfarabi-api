using System.ComponentModel.DataAnnotations.Schema;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Models
{
    public class User
    {  
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public GenderEnum Gender { get; set; }
        public RoleEnum Role { get; set; }

        public List<GroupUser> ?GroupUsers { get; set; }

        public bool IsLogIn { get; set; } = false;

        [NotMapped]
        public static string TOKEN = "sdfnjksdbfjknsdjkncsjdncjksndjkcsdnjkcbjkbcjksdlksjknjksdn";
    }
}
