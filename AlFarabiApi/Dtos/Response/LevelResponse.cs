using AlFarabiApi.Models;

namespace AlFarabiApi.Dtos.Response
{
    public class LevelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static LevelResponse Create(Level level)
        {
            return new LevelResponse
            {
                Id = level.Id,
                Name = level.Name,
              
            };
        }
        public static LevelResponse? CreateNullable(Level? level)
        {
            if (level is null)
            {
                return null;
            }
            else
            {
                return Create(level);
            }
        }
    }
}
