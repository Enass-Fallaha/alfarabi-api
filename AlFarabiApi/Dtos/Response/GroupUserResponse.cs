using System.ComponentModel.DataAnnotations;
using AlFarabiApi.Models;

namespace AlFarabiApi.Dtos
{
    public class GroupUserResponse
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public double Cost { get; set; }
        public UserResponse? User { get; set; }
        public GroupResponse? Group { get; set; }
        public DateTime CreatedAt { get; set; } 

        public static GroupUserResponse Create(GroupUser groupuser)
        {
            return new GroupUserResponse
            {
                UserId = groupuser.UserId,
                GroupId = groupuser.GroupId,
                Cost = groupuser.Cost,
                User = UserResponse.CreateNullable(groupuser.User),
                Group = GroupResponse.CreateNullable(groupuser.Group),
                CreatedAt = DateTime.Now

            };
        }

    }


}

