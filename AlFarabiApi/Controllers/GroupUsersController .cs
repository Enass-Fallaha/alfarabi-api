using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Models;
using AlFarabiApi.Enums;

namespace AlFarabiApi.Controllers
{
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class GroupUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupUsersController ( ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpGet ("get-users-by-group-id")]
        public async Task<IActionResult> GetByGroupIdAsync ( [FromQuery]int groupId ,[FromQuery] RoleEnum role )
        {
            var group = await _context.Groups.FindAsync (groupId);
            if ( group == null )
            {
                return NotFound ($"Group with ID = {groupId} is not found.");
            }

            var users = await _context.GroupUsers
                .Where (gu => gu.GroupId == groupId && gu.User.Role == role)
                .Select (gu => gu.User)
                .ToListAsync (); 

            return Ok (UserResponse.CreateFromList (users));
        }


        [HttpGet ("get-by-userid-and-groupid")]
        public async Task<IActionResult> GetByIdAsync ( int userId , int groupId )
        {
            var groupUser = await _context.GroupUsers
                .Include (gu => gu.User)
                .Include (gu => gu.Group.Level)
                .FirstOrDefaultAsync (gu => gu.UserId == userId && gu.GroupId == groupId);

            if ( groupUser == null )
            {
                return NotFound ();
            }

            return Ok (GroupUserResponse.Create (groupUser));
        }

        [HttpPost ("add-user-to-group")]
        public async Task<IActionResult> CreateAsync ( AddUserToGroupRequests dto )
        {
            var existingGroupUser = await _context.GroupUsers
                .FirstOrDefaultAsync (gu => gu.UserId == dto.UserId && gu.GroupId == dto.GroupId);

            if ( existingGroupUser != null )
            {
                return Ok (new { message = "The User already exists." });
            }

            var groupUser = new GroupUser
            {
                UserId = dto.UserId ,
                GroupId = dto.GroupId ,
                Cost = dto.Cost ,
                CreatedAt = DateTime.UtcNow 
            };

            await _context.GroupUsers.AddAsync (groupUser);
            await _context.SaveChangesAsync ();

            return await GetByIdAsync (groupUser.UserId , groupUser.GroupId);
        }

        [HttpPut ("update-user-in-group")]
        public async Task<IActionResult> UpdateAsync ( AddUserToGroupRequests dto )
        {
            var groupUser = await _context.GroupUsers
                .FirstOrDefaultAsync (gu => gu.UserId == dto.UserId && gu.GroupId == dto.GroupId);

            if ( groupUser == null )
            {
                return NotFound ();
            }

            groupUser.Cost = dto.Cost;
            await _context.SaveChangesAsync ();

            return await GetByIdAsync (groupUser.UserId , groupUser.GroupId);
        }

        [HttpDelete ("delete-user-from-group")]
        public async Task<IActionResult> DeleteAsync ( int userId , int groupId )
        {
            var groupUser = await _context.GroupUsers
                .FirstOrDefaultAsync (gu => gu.UserId == userId && gu.GroupId == groupId);

            if ( groupUser == null )
            {
                return NotFound ();
            }

            _context.GroupUsers.Remove (groupUser);
            await _context.SaveChangesAsync ();

            return NoContent ();
        }
    }
}