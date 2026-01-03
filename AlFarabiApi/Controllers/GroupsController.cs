using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Models;

namespace AlFarabiApi.Controllers
{
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupsController ( ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync ( int? levelId = null )
        {
            var query = _context.Groups
                .Include (g => g.Level)
                .AsQueryable ();

            if ( levelId is not null )
            {
                query = query.Where (g => g.LevelId == levelId);
            }

            var groupList = await query.ToListAsync ();
            var groupResponse = groupList.Select (g => GroupResponse.Create (g));

            return Ok (groupResponse);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetByIdAsync ( int id )
        {
            var group = await _context.Groups
                .Include (g => g.Level)
                .FirstOrDefaultAsync (g => g.Id == id);

            if ( group == null )
            {
                return NotFound ();
            }

            return Ok (GroupResponse.Create (group));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync ( CreateGroupRequest dto )
        {
            var existingGroup = await _context.Groups
                .FirstOrDefaultAsync (g =>
                    g.Name == dto.Name &&
                    g.LevelId == dto.LevelId &&
                    g.Gender == dto.Gender);

            if ( existingGroup != null )
            {
                return Ok (new { message = "The Group already exists" });
            }

            var group = new Group
            {
                Gender = dto.Gender ,
                Name = dto.Name ,
                LevelId = dto.LevelId
            };

            await _context.Groups.AddAsync (group);
            await _context.SaveChangesAsync ();

            return await GetByIdAsync (group.Id);
        }

        [HttpPut ("{id:int}")]
        public async Task<IActionResult> UpdateAsync ( int id , CreateGroupRequest dto )
        {
            var group = await _context.Groups.FindAsync (id);
            if ( group == null )
            {
                return NotFound ($"Group with ID = {id} is not found.");
            }

            var duplicateGroup = await _context.Groups
                .FirstOrDefaultAsync (g =>
                    g.Name == dto.Name &&
                    g.LevelId == dto.LevelId &&
                    g.Gender == dto.Gender &&
                    g.Id != id);

            if ( duplicateGroup != null )
            {
                return Ok (new { message = "The Group already exists" });
            }

            group.Name = dto.Name;
            group.LevelId = dto.LevelId;
            group.Gender = dto.Gender;

            await _context.SaveChangesAsync ();
            return await GetByIdAsync (group.Id);
        }

        [HttpDelete ("{id:int}")]
        public async Task<IActionResult> DeleteAsync ( int id )
        {
            var group = await _context.Groups.FindAsync (id);
            if ( group == null )
            {
                return NotFound ($"Group with ID = {id} is not found.");
            }

            _context.Groups.Remove (group);
            await _context.SaveChangesAsync ();

            return NoContent ();
        }
    }
}