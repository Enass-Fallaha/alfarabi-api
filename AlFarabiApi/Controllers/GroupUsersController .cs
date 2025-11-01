using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Models;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GroupUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GroupUsersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("get-users-by-group-id")]
        public async Task<IActionResult> GetByGroupIdAsync(int groupId, RoleEnum role)
        {
            var group =
            await _context.Groups.FindAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }
            var listUser = await _context.GroupUsers
                .Where(gu => gu.GroupId == groupId && gu.User.Role==role)
                .Select(gu =>gu.User).ToListAsync();
            return Ok(UserResponse.CreateFromList(listUser));
        }



        [HttpGet("get-groups-by-user-id")]

        public async Task<IActionResult> GetByUserIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) { return NotFound(); }

          var listGroup =  await _context.GroupUsers
                .Where(gu => gu.UserId == userId)
                .Select(gu=> gu.Group).ToListAsync();

            return Ok(GroupResponse.CreateFromList(listGroup));

          
        }


        [HttpGet("get-by-userid-and-groupid")]
        public async Task<IActionResult> GetByIdAsync(int userId, int groupId)
        {
            var groupUser =
            await _context.GroupUsers
            .Include(gu => gu.User)
            .Include( gu=> gu.Group.Level )       
            .FirstOrDefaultAsync(gu => gu.UserId == userId && gu.GroupId == groupId);

            if (groupUser == null)
            {
                return NotFound();
            }

            return Ok(GroupUserResponse.Create(groupUser));
        }


        [HttpPost("add-user-to-group")]
        public async Task<IActionResult> CreateAsync( AddUserToGroupRequestcs dto)

        {    var groupUser= await _context.GroupUsers
                           .FirstOrDefaultAsync(gu => gu.UserId == dto.UserId && gu.GroupId==dto.GroupId);
            if (groupUser != null) { return Ok(new { message = "The User already exists " }); }
            groupUser = new GroupUser
            {
                UserId = dto.UserId,
                GroupId = dto.GroupId,
                Cost = dto.Cost,
                CreatedAt = DateTime.Now
            };
            await _context.GroupUsers.AddAsync(groupUser);
            _context.SaveChanges();

            return await GetByIdAsync(groupUser.UserId,groupUser.GroupId);
        }


        [HttpPut()]
        public async Task<IActionResult> UpdateAsync( AddUserToGroupRequestcs dto)
        {
            var groupUser = await _context.GroupUsers
                .FirstOrDefaultAsync(gu => gu.UserId == dto.UserId && gu.GroupId == dto.GroupId);

            if (groupUser == null) { return NotFound(); }
             groupUser.Cost = dto.Cost;
            _context.SaveChanges();
            return await GetByIdAsync(groupUser.UserId,groupUser.GroupId);

        }


        [HttpDelete("delete-user-from-group")]
        public async Task<IActionResult> DeleteAsync(int userId,int groupId)
        {
            var groupUser = await _context.GroupUsers
                .FirstOrDefaultAsync(gu => gu.UserId==userId && gu.GroupId==groupId);
            if (groupUser == null) { return NotFound(); }
            ;
            _context.GroupUsers.Remove(groupUser);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
