using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Enums;
using AlFarabiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlFarabiApi.Controllers
{
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController ( ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync ( [FromQuery] RoleEnum? role )
        {
            if ( role == null )
                return BadRequest ("Role is required.");

            if ( role != RoleEnum.Teacher && role != RoleEnum.Student )
                return BadRequest ("Invalid role. Allowed values: Teacher, Student.");

            var users = await _context.Users
                .Where (u => u.Role == role)
                .OrderBy (u => u.Name)
                .Include(u => u.GroupUsers)!
                .ThenInclude(ul => ul.Group)
                .ThenInclude(ul => ul .Level)
                .Select (u => UserResponse.Create (u))
                .ToListAsync ();

            return Ok (users);
        }

        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetByIdAsync ( int id )
        {
            var user = await _context.Users.FindAsync (id);
            if ( user == null )
                return NotFound ();

            return Ok (UserResponse.Create (user));
        }

        [HttpPost]
        public async Task<IActionResult> AddAllAsync ( CreateUserRequest dto )
        {
            if ( await _context.Users.AnyAsync (u => u.Email == dto.Email) )
            {
                return BadRequest (new { message = "The User already exists" });
            }

            var user = new User
            {
                Name = dto.Name ,
                Email = dto.Email ,
                Password = dto.Password ,
                Phone = dto.Phone ,
                Role = dto.Role ,
                Gender = dto.Gender
            };

            await _context.Users.AddAsync (user);
            await _context.SaveChangesAsync (); 

            return Ok (UserResponse.Create (user));
        }

        [HttpPut ("{id:int}")]
        public async Task<IActionResult> UpdateAsync ( int id , [FromBody] CreateUserRequest dto )
        {
            var user = await _context.Users.FindAsync (id);
            if ( user == null )
                return NotFound ();

            
            if ( await _context.Users.AnyAsync (u => u.Email == dto.Email && u.Id != id) )
            {
                return BadRequest (new { message = "A user with this email already exists." });
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.Phone = dto.Phone;
            user.Role = dto.Role;
            user.Gender = dto.Gender;

            await _context.SaveChangesAsync (); 
            return Ok (UserResponse.Create (user));
        }

        [HttpDelete ("{id:int}")]
        public async Task<IActionResult> DeleteAsync ( int id )
        {
            var user = await _context.Users.FindAsync (id);
            if ( user == null )
                return NotFound ($"User with ID = {id} is not found.");

            _context.Users.Remove (user);
            await _context.SaveChangesAsync (); 
            return NoContent ();
        }
    }
}