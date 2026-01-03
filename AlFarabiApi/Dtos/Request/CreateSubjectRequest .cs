namespace AlFarabiApi.Dtos.Request
{
    public class CreateSubjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public int LevelId { get; set; }
        public List<int>? UserIds { get; set; }

    }
}
