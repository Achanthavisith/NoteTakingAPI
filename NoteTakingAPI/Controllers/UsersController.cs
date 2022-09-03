using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTakingAPI.Models;
using NuGet.Packaging.Signing;
using NuGet.Protocol;

namespace NoteTakingAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NoteDataContext _context;

        public UsersController(NoteDataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] string? name, int? id)
        {

            if (_context.Users == null)
            {
                return NotFound();
            }

            var User = await _context.Users.ToListAsync();

            if (name != null && id != null) 
            {
                User = await _context.Users.Where(p => p.Name == name).Where(x=> x.UserId == id).ToListAsync();
            } 
            else if (name != null)
            {
                User = await _context.Users.Where(p => p.Name == name).ToListAsync();
            }
            else if (id != null)
            {
                User = await _context.Users.Where(x => x.UserId == id).ToListAsync();
            }

            if (User.Count == 0)   
            {
                return Problem("User with those parameters not found, or table is empty");
            } 
            else
            {
                return User;
            }
        }
    
        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserIdExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'NoteDataContext.Users'  is null.");
            }

            _context.Users.Add(user);

            if (user.Email == "" || user.Password == "" || user.Name == "" || user.Password == "" || user.LastName == "")
            {
                return Problem("All fields required.");
            }
            else if (UserExists(user.Email))
            {
                return Problem("User already exists");
            }
            else
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string email)
        {
            return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
        }

        private bool UserIdExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }


    }
}
