using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Models;

namespace AlFarabiApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync( int? levelId = null )
        {
            var qury =
                 _context.Groups
                .Include(g => g.Level) 
                .AsQueryable();

            if(levelId is not null)
            {
                qury = qury.Where( o =>  o.LevelId == levelId  );

            }

        var groupList = await qury.ToListAsync();

            var groupResponse = groupList.Select(g => GroupResponse.Create(g));

            return Ok(groupResponse);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var group =
            await _context.Groups
            .Include(g => g.Level)
            .FirstOrDefaultAsync(g => g.Id==id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(GroupResponse.Create(group));
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGroupRequest dto)
        {
            var group = await _context.Groups
                .FirstOrDefaultAsync(g=>g.Name==dto.Name&& g.LevelId==dto.LevelId && g.Gender ==dto.Gender );
            if (group != null) 
                return Ok(new { message = "The Group already exists" } ); 

             group = new Group
            {
                Gender = dto.Gender,
                Name = dto.Name,
                LevelId = dto.LevelId
            };
            await _context.Groups.AddAsync(group);
            _context.SaveChanges();

            return await GetByIdAsync(group.Id);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CreateGroupRequest dto)
        {


            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.Name == dto.Name && g.LevelId == dto.LevelId);
            if (group != null)
               return Ok(new { message = "The Group already exists" }); 
             group = await _context.Groups.FindAsync(id);
            if (group == null) 
               return NotFound(); 

            group.Name = dto.Name;
            group.LevelId = dto.LevelId;
            group.Gender = dto.Gender;
            _context.SaveChanges(); 
               return await GetByIdAsync(group.Id);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
             return NotFound(); 
            ;
            _context.Groups.Remove(group);
            _context.SaveChanges();
             return NoContent();
        }
    }
}
