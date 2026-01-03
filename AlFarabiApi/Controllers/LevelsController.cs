using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Dtos.Response;
using AlFarabiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlFarabiApi.Controllers
{
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LevelsController ( ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync ( )
        {
            var levels = await _context.Levels
                .Select (l => LevelResponse.Create (l))
                .ToListAsync ();

            return Ok (levels);
        }

        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetByIdAsync ( int id )
        {
            var level = await _context.Levels.FindAsync (id);
            if ( level == null )
            {
                return NotFound ($"Level with ID = {id} is not found.");
            }

            return Ok (LevelResponse.Create (level));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync ( CreateLevelRequest dto )
        {
            var existingLevel = await _context.Levels
                .FirstOrDefaultAsync (l => l.Name == dto.Name);

            if ( existingLevel != null )
            {
                return Ok (new { message = "The Level already exists." });
            }

            var level = new Level
            {
                Name = dto.Name
            };

            await _context.Levels.AddAsync (level);
            await _context.SaveChangesAsync ();

            return Ok (LevelResponse.Create (level));
        }

        [HttpPut ("{id:int}")]
        public async Task<IActionResult> UpdateAsync ( int id , CreateLevelRequest dto )
        {
            var level = await _context.Levels.FindAsync (id);
            if ( level == null )
            {
                return NotFound ($"Level with ID = {id} is not found.");
            }

            var duplicateLevel = await _context.Levels
                .FirstOrDefaultAsync (l => l.Name == dto.Name && l.Id != id);

            if ( duplicateLevel != null )
            {
                return Ok (new { message = "The Level already exists." });
            }

            level.Name = dto.Name;
            await _context.SaveChangesAsync ();

            return Ok (LevelResponse.Create (level));
        }

        [HttpDelete ("{id:int}")]
        public async Task<IActionResult> DeleteAsync ( int id )
        {
            var level = await _context.Levels.FindAsync (id);
            if ( level == null )
            {
                return NotFound ($"Level with ID = {id} is not found.");
            }

            var hasGroups = await _context.Groups.AnyAsync (g => g.LevelId == id);
            var hasSubjects = await _context.Subjects.AnyAsync (s => s.LevelId == id);

            if ( hasGroups || hasSubjects )
            {
                return BadRequest (new
                {
                    message = "This level cannot be deleted because there are groups or subjects associated with it."
                });
            }

            _context.Levels.Remove (level);
            await _context.SaveChangesAsync ();

            return NoContent ();
        }
    }
}