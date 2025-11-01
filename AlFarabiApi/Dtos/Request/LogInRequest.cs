using System.ComponentModel.DataAnnotations;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Dtos.Request
{
    public class LogInRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
