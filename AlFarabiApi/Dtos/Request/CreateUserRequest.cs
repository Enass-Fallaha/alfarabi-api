using System.ComponentModel.DataAnnotations;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Dtos.Request
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public RoleEnum Role { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
