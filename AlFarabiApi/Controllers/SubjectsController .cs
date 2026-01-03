using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlFarabiApi.Dtos;
using AlFarabiApi.Dtos.Request;
using AlFarabiApi.Models;
using AlFarabiApi.Dtos.Response;

namespace AlFarabiApi.Controllers
{
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController ( ApplicationDbContext context )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync ( )
        {
            var subjects = await _context.Subjects
                .Include (s => s.Level)!
                .Include (s => s.SubjectUsers)!
                .ThenInclude (su => su.User)
                .Select (s => SubjectResponse.Create (s))
                .ToListAsync ();

            return Ok (subjects);
        }

        [HttpGet ("{id:int}")]
        public async Task<IActionResult> GetByIdAsync ( int id )
        {
            var subject = await _context.Subjects
                .Include (s => s.Level)!
                .Include (s => s.SubjectUsers)!
                .ThenInclude (su => su.User)
                .FirstOrDefaultAsync (s => s.Id == id);

            if ( subject == null )
            {
                return NotFound ($"Subject with ID = {id} is not found.");
            }

            return Ok (SubjectResponse.Create (subject));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync ( CreateSubjectRequest dto )
        {
            var existingSubject = await _context.Subjects
                .FirstOrDefaultAsync (s => s.Name == dto.Name && s.LevelId == dto.LevelId);

            if ( existingSubject != null )
            {
                return Ok (new { message = "The Subject already exists" });
            }

            var subject = new Subject
            {
                Name = dto.Name ,
                LevelId = dto.LevelId
            };

            await _context.Subjects.AddAsync (subject);
            await _context.SaveChangesAsync (); 

            if ( dto.UserIds?.Count > 0 )
            {
                var subjectTeachers = new List<SubjectUser> ();
                foreach ( var userId in dto.UserIds )
                {
                    var subjectTeacher = new SubjectUser
                    {
                        SubjectId = subject.Id ,
                        UserId = userId
                    };
                    subjectTeachers.Add (subjectTeacher);
                }

                await _context.SubjectUsers.AddRangeAsync (subjectTeachers);
                await _context.SaveChangesAsync (); 
            }

            return await GetByIdAsync (subject.Id);
        }

        [HttpPut ("{id:int}")]
        public async Task<IActionResult> UpdateAsync ( int id , CreateSubjectRequest dto )
        {
            var subject = await _context.Subjects.FindAsync (id);
            if ( subject == null )
            {
                return NotFound ($"Subject with ID = {id} is not found.");
            }

            var conflictingSubject = await _context.Subjects
                .FirstOrDefaultAsync (s => s.Name == dto.Name && s.LevelId == dto.LevelId && s.Id != id);

            if ( conflictingSubject != null )
            {
                return Ok (new { message = "The Subject already exists" });
            }

            subject.Name = dto.Name;
            subject.LevelId = dto.LevelId;

            var existingSubjectUsers = await _context.SubjectUsers
                .Where (su => su.SubjectId == id)
                .ToListAsync ();

            _context.SubjectUsers.RemoveRange (existingSubjectUsers);

            if ( dto.UserIds?.Count > 0 )
            {
                var subjectTeachers = new List<SubjectUser> ();
                foreach ( var userId in dto.UserIds )
                {
                    subjectTeachers.Add (new SubjectUser
                    {
                        SubjectId = subject.Id ,
                        UserId = userId
                    });
                }

                await _context.SubjectUsers.AddRangeAsync (subjectTeachers);
            }

            await _context.SaveChangesAsync (); 
            return await GetByIdAsync (subject.Id);
        }

        [HttpDelete ("{id:int}")]
        public async Task<IActionResult> DeleteAsync ( int id )
        {
            var subject = await _context.Subjects.FindAsync (id);
            if ( subject == null )
            {
                return NotFound ($"Subject with ID = {id} is not found.");
            }

            var hasAssociatedTeachers = await _context.SubjectUsers.AnyAsync (su => su.SubjectId == id);
            if ( hasAssociatedTeachers )
            {
                return BadRequest (new
                {
                    message = "This Subject cannot be deleted because there are Teachers associated with it."
                });
            }

            _context.Subjects.Remove (subject);
            await _context.SaveChangesAsync (); 
            return NoContent ();
        }
    }
}