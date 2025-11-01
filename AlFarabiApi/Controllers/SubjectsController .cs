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
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var subject = await _context.Subjects
                .Include( s =>s.Level)
                .Select(s => SubjectResponse.Create(s))
                .ToListAsync();

            return Ok(subject);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var subject =
            await _context.Subjects
            .Include(s => s.Level)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
            {
                return NotFound();
            }

            return Ok(SubjectResponse.Create(subject));
        }





        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateSubjectRequest dto)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Name == dto.Name && s.LevelId == dto.LevelId);
            if (subject != null) 
            { return Ok(new { message = "The Subject already exists" }); }

             subject = new Subject
            {
                Name = dto.Name,
                LevelId = dto.LevelId
              
            };
            await _context.Subjects.AddAsync(subject);
            _context.SaveChanges();
            return await GetByIdAsync(subject.Id);
            
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, CreateSubjectRequest dto)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Name == dto.Name && s.LevelId == dto.LevelId);
            if ( subject != null) 
                { return Ok(new { message = "The Subject already exists" }); }
                subject = await _context.Subjects.FindAsync(id);
            if (subject == null) 
                { return NotFound(); }
            subject.Name = dto.Name;
            subject.LevelId = dto.LevelId;
            _context.SaveChanges();
            return await GetByIdAsync(subject.Id);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) { return NotFound(); }
            ;
            _context.Subjects.Remove(subject);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
