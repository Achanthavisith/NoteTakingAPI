using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTakingAPI.Models;

namespace NoteTakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNamesController : ControllerBase
    {
        private readonly NoteDataContext _context;

        public UserNamesController(NoteDataContext context)
        {
            _context = context;
        }

        // GET: api/UserNames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserNames>>> GetUserNames()
        {
            if (_context.UserNames == null)
            {
                return NotFound();
            }
            return await _context.UserNames.ToListAsync();
        }

        // GET: api/UserNames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserNames>> GetUserNames(int id)
        {
            if (_context.UserNames == null)
            {
                return NotFound();
            }
            var userNames = await _context.UserNames.FindAsync(id);

            if (userNames == null)
            {
                return NotFound();
            }

            return userNames;
        }

        // PUT: api/UserNames/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserNames(int id, UserNames userNames)
        {
            if (id != userNames.UserNamesId)
            {
                return BadRequest();
            }

            _context.Entry(userNames).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserNamesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserNames
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserNames>> PostUserNames(UserNames userNames)
        {
            if (_context.UserNames == null)
            {
                return Problem("Entity set 'NoteDataContext.UserNames'  is null.");
            }
            _context.UserNames.Add(userNames);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserNamesExists(userNames.UserNamesId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserNames", new { id = userNames.UserNamesId }, userNames);
        }

        // DELETE: api/UserNames/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserNames(int id)
        {
            if (_context.UserNames == null)
            {
                return NotFound();
            }
            var userNames = await _context.UserNames.FindAsync(id);
            if (userNames == null)
            {
                return NotFound();
            }

            _context.UserNames.Remove(userNames);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserNamesExists(int id)
        {
            return (_context.UserNames?.Any(e => e.UserNamesId == id)).GetValueOrDefault();
        }
    }
}
