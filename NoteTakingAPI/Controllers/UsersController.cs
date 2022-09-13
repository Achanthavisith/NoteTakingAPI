using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteTakingAPI.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace NoteTakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NoteDataContext _context;

        public UsersController(NoteDataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet, Authorize]
        public async Task<ActionResult<List<User>>> GetUsers([FromQuery] string? name, int? id)
        {

            if (_context.Users == null)
            {
                return NotFound();
            }

            var User = await _context.Users.ToListAsync();

            if (name != null && id != null)
            {
                User = await _context.Users
                    .Where(p => p.Name == name)
                    .Where(x => x.UserId == id)
                    .ToListAsync();
            }
            else if (name != null)
            {
                User = await _context.Users
                    .Where(p => p.Name == name)
                    .ToListAsync();
            }
            else if (id != null)
            {
                User = await _context.Users
                    .Where(x => x.UserId == id)
                    .ToListAsync();
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
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
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

            return user;
        }

        // PUT: api/Users/5
        /* too much work to get every field for user changes
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

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
        */

        public static readonly User user = new();//new user object for the registration

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserReg userReg)//use the userReg class to use plain text password/etc and other fields to mimic the user model/class
        {

            if (_context.Users == null)
            {
                return Problem("Entity set 'NoteDataContext.Users'  is null.");
            }
            if (UserExists(userReg.Email))
            {
                return Problem("User already exists");
            }
            else if (userReg.Email == "" || userReg.Password == "" || userReg.Name == "" || userReg.Password == "" || userReg.LastName == "")
            {
                return Problem("All fields required.");
            }
            else
            {
                try
                {
                    CreatedPasswordHash(userReg.Password, out byte[] passwordHash, out byte[] passwordSalt);//hash the password
                    //covert all fields for data insertion
                    user.Email = userReg.Email;
                    user.Name = userReg.Name;
                    user.LastName = userReg.LastName;
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    //add the user and save 
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return Problem("Check Parameters or fields");//probably redundant try block
                }
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(UserReg userReg)//use the userReg class to use plain text password/etc and other fields to mimic the user model/class
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'NoteDataContext.Users'  is null.");
            }
            if (!UserExists(userReg.Email))
            {
                return Problem("Password or email is incorrect.");
            }
            else if (userReg.Email == "" || userReg.Password == "")
            {
                return Problem("All fields required.");
            }
            else
            {
                //get the user by email//firstOrDefaultmethod returns the first row back.
                var user = await _context.Users
                    .Where(user => user.Email == userReg.Email)
                    .FirstOrDefaultAsync();

                if (!VerifyPasswordHash(userReg.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return Problem("Password or email is incorrect.");
                }
                else
                {
                    string token = CreateToken(user);
                    return Ok(token);
                }
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}"), Authorize]
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

        private static void CreatedPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();

            passwordSalt = hmac.Key;//generates a key to use for hash

            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));//use key to hash the password string.
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);//gets the password key to compute hash

            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));//use key to compute the hash 

            return computeHash.SequenceEqual(passwordHash);// checks if the entered password hash matches the db hash
        }

        private static string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var secretKey = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("Jwt")["Token"];

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
