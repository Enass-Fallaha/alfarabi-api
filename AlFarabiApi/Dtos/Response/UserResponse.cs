using System.ComponentModel.DataAnnotations;
using AlFarabiApi.Enums;
using AlFarabiApi.Models;

namespace AlFarabiApi.Dtos
{
    public class UserResponse
    {
        [MaxLength(100)]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
       // public string Password { get; set; }
        public string Phone { get; set; }
        public RoleEnum Role { get; set; }
        public GenderEnum Gender { get; set; }
        public List<GroupResponse>? group { get; set; }

        public bool IsLogIn { get; set; }

        public static UserResponse Create(User user)
        {
            return new UserResponse
            {
                Id = user.Id ,
                Name = user.Name ,
                Email = user.Email ,
                //  Password = user.Password,
                Phone = user.Phone ,
                Role = user.Role ,
                Gender = user.Gender ,
                group = user.GroupUsers?.Select( o => GroupResponse.CreateNullable(o.Group)).ToList() !
            };
        }

        public static UserResponse? CreateNullable(User? user)
        {
            if(user is null)
            {
                return null;
            }
            else
            {
                return Create(user);
            }
        }

        public static List<UserResponse?> CreateFromList(List<User> users)
        {

            if (users== null)
            {
                return [];
            }
            return
            users.Select(
                o => CreateNullable(o)
            ).ToList();

        }
    }
}
