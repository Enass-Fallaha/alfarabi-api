namespace AlFarabiApi.Dtos.Request
{
    public class AddUserToGroupRequests
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public double Cost { get; set; }
    }
}
