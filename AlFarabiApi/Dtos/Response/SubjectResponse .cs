using AlFarabiApi.Models;

namespace AlFarabiApi.Dtos.Response
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public List<UserResponse>? Teachers { get; set; }
        public LevelResponse? Level { get; set; }

        public static SubjectResponse Create(Subject subject)
        {
            return new SubjectResponse
            {
                Id = subject.Id,
                Name = subject.Name,
                LevelId = subject.LevelId,
                Level = LevelResponse.CreateNullable(subject.Level),
                Teachers = subject.SubjectUsers?.Select(o => UserResponse.CreateNullable (o.User)).ToList()!
            };
        }
        public static SubjectResponse? CreateNullable(Subject? subject)
        {
            if (subject is null)
            {
                return null;
            }
            else
            {
                return Create(subject);
            }
        }
    }
}
