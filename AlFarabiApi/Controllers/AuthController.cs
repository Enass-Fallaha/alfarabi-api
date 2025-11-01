using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Dtos;
using AlFarabiApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AlFarabiApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {  
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("log-in")]
        public async Task<IActionResult> LogInAsync(LogInRequest dto)
        {
            var user =
            await _context.Users
                .FirstOrDefaultAsync(
                user => user.Email == dto.Email
                );


            if (user == null || user.Password != dto.Password)
            {
                return BadRequest(
                    new
                    {
                        message = "Email or password incorrect"
                    }
                );
            }


            user.IsLogIn = true;
            _context.SaveChanges();

            return Ok(
                new
                {
                    isLogIn = true,
                    token = user.Role == Enums.RoleEnum.Admin ? Models.User.TOKEN : null,
                    user.Id,
                    user.Name

                }
            );
        }
      
        [HttpPost("log-out")]
        public async Task<IActionResult> LogIAsync(int userId)
        {
            var user =
            await _context.Users
                .FirstOrDefaultAsync(
                user => user.Id == userId
                );


            if (user == null )
            {
                return NotFound();
                
            }

            user.IsLogIn = false;
            _context.SaveChanges();
            return Ok();
        }

    }
}
