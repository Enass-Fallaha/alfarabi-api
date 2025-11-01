//using AlFarabiApi.Models;
using System.ComponentModel.DataAnnotations;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Dtos.Request
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public int LevelId { get; set; }
        public GenderEnum Gender { get; set; }

    }
}
