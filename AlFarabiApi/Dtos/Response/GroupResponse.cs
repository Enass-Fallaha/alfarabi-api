using System.ComponentModel.DataAnnotations;
using AlFarabiApi.Dtos.Response;
using AlFarabiApi.Enums;
using AlFarabiApi.Models;

namespace AlFarabiApi.Dtos
{
    public class GroupResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int LevelId { get; set; }
        public LevelResponse? Level { get; set; }
        public GenderEnum Gender { get; set; }
        public static GroupResponse Create(Group group)
        {
            return new GroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                LevelId = group.LevelId,
                Gender = group.Gender,
                Level = LevelResponse.CreateNullable(group.Level)



            };
        }

        public static List<GroupResponse?> CreateFromList(List<Group> groups)
        {
      
            if (groups == null)
            {
                return [];
            }
            return
            groups.Select(
                o => CreateNullable(o)
            ).ToList();

        }
        public static GroupResponse? CreateNullable(Group? group)
        {
            if (group is null)
            {
                return null;
            }
            else
            {
                return Create(group);
            }
        }



    }


}

