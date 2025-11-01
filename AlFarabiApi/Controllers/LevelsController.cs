using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Models;
using AlFarabiApi.Dtos.Response;

namespace AlFarabiApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var level = await _context.Levels
                .Select(l => LevelResponse.Create(l))
                .ToListAsync();

            return Ok(level);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var level =
            await _context.Levels.FindAsync(id);

            if (level == null)
            {
                return NotFound();
            }

            return Ok(LevelResponse.Create(level));
        }





        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateLevelRequest dto)
        {
            var level = await _context.Levels
                .FirstOrDefaultAsync(l => l.Name == dto.Name);
            if (level != null)
               return Ok(new {message = "The Level already exists" }); 
             level = new Level
            {
                Name = dto.Name
              
            };
            await _context.Levels.AddAsync(level);
            _context.SaveChanges();
            return Ok(LevelResponse.Create(level)
            );
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CreateLevelRequest dto)
        {
          
            var level = await _context.Levels
                .FirstOrDefaultAsync( l => l.Name == dto.Name);
            if (level != null) 
               return Ok(new { message = "The Level already exists" }); 
            level = await _context.Levels.FindAsync(id);
            if (level == null)
               return NotFound(); 
            level.Name = dto.Name;
            _context.SaveChanges();
               return Ok(LevelResponse.Create(level));

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level == null) 
               return NotFound(); 
            
            var countgroup = await _context.Groups.CountAsync(g=> g.LevelId == id );
            var countsubject = await _context.Subjects.CountAsync(s => s.LevelId == id);
            if(countgroup > 0 || countsubject > 0) 
               return BadRequest(new {message = "This level cannot be deleted because there are groups or subjects associated with it." }); 
            _context.Levels.Remove(level);
            _context.SaveChanges();
               return NoContent();
        } 
    }
}
