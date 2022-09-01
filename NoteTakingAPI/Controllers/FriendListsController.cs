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
    public class FriendListsController : ControllerBase
    {
        private readonly NoteDataContext _context;

        public FriendListsController(NoteDataContext context)
        {
            _context = context;
        }

        // GET: api/FriendLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendList>>> GetFriendLists()
        {
          if (_context.FriendLists == null)
          {
              return NotFound();
          }
            return await _context.FriendLists.ToListAsync();
        }

        // GET: api/FriendLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FriendList>> GetFriendList(int id)
        {
          if (_context.FriendLists == null)
          {
              return NotFound();
          }
            var friendList = await _context.FriendLists.FindAsync(id);

            if (friendList == null)
            {
                return NotFound();
            }

            return friendList;
        }

        // PUT: api/FriendLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendList(int id, FriendList friendList)
        {
            if (id != friendList.FriendListId)
            {
                return BadRequest();
            }

            _context.Entry(friendList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendListExists(id))
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

        // POST: api/FriendLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FriendList>> PostFriendList(FriendList friendList)
        {
          if (_context.FriendLists == null)
          {
              return Problem("Entity set 'NoteDataContext.FriendLists'  is null.");
          }

            try
            {
                _context.FriendLists.Add(friendList);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFriendList", new { id = friendList.FriendListId }, friendList);
            }
            catch
            {
                return Problem($"User id of {friendList.Friends} does not exist, cannot add a this user.");
            }

        }

        // DELETE: api/FriendLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriendList(int id)
        {
            if (_context.FriendLists == null)
            {
                return NotFound();
            }
            var friendList = await _context.FriendLists.FindAsync(id);
            if (friendList == null)
            {
                return NotFound();
            }

            _context.FriendLists.Remove(friendList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FriendListExists(int id)
        {
            return (_context.FriendLists?.Any(e => e.FriendListId == id)).GetValueOrDefault();
        }
    }
}
