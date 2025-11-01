using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Enums;
using AlFarabiApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlFarabiApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync (RoleEnum role)
        {   
            if (role == RoleEnum.Teacher)
            {
                var user = await _context.Users
                .Where(u => u.Role == RoleEnum.Teacher)
                .Select(u => UserResponse.Create(u)).ToListAsync();
                user = user.OrderBy(u => u.Name).ToList();
                return Ok(user);
            }
            else if (role == RoleEnum.Student)
            {
                var user = await _context.Users
                  .Where(u => u.Role == RoleEnum.Student)
                  .Select(u => UserResponse.Create(u)).ToListAsync();
                user = user.OrderBy(u => u.Name).ToList();
                return Ok(user);
            }
        
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(UserResponse.Create(user));

        }

        [HttpPost]
        public async Task<IActionResult> AddAllAsync(CreateUserRequest dto)
        {  
            var user= await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user != null)
                 return BadRequest(new { message = "The User already exists" });
             user = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Phone = dto.Phone,
                Role = dto.Role
            };
          await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return Ok(UserResponse.Create(user));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateAsync(int id , [FromBody]CreateUserRequest dto)
        {
           var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            var userafteredit = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && user.Email!=dto.Email);
            if (userafteredit != null)
                return BadRequest(new { message = "The User already exists" }); 
            
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.Phone = dto.Phone;
            user.Role = dto.Role;
            user.Gender = dto.Gender;

            _context.SaveChanges();
            return Ok(UserResponse.Create(user));
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"This Id = {id} Is Not Found");
                _context.Users.Remove(user);
                 _context.SaveChanges();
            return NoContent();
        }
    }
}
