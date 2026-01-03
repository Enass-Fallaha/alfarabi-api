using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Dtos;
using AlFarabiApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AlFarabiApi.Controllers
{
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController ( ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpPost ("log-in")]
        public async Task<IActionResult> LogInAsync ( LogInRequest dto )
        {
            var user = await _context.Users
                .FirstOrDefaultAsync (u => u.Email == dto.Email);

            if ( user == null || user.Password != dto.Password )
            {
                return BadRequest (new { message = "Email or password incorrect" });
            }

            user.IsLogIn = true;
            await _context.SaveChangesAsync ();

            return Ok (new
            {
                isLogIn = true ,
                token = user.Role == Enums.RoleEnum.Admin ? Models.User.TOKEN : null ,
                user.Id ,
                user.Name
            });
        }

        [HttpPost ("log-out")]
        public async Task<IActionResult> LogOutAsync ( int userId )
        {
            var user = await _context.Users
                .FirstOrDefaultAsync (u => u.Id == userId);

            if ( user == null )
            {
                return NotFound ();
            }

            user.IsLogIn = false;
            await _context.SaveChangesAsync();

            return Ok ();
        }
    }
}