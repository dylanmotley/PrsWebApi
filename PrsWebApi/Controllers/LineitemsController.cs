using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsWebApi.Data;
using PrsWebApi.Models;

namespace PrsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineitemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LineitemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Lineitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lineitem>>> GetLineitems()
        {
            return await _context.Lineitems.ToListAsync();
        }

        // GET: api/Lineitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lineitem>> GetLineitem(int id)
        {
            var lineitem = await _context.Lineitems.FindAsync(id);

            if (lineitem == null)
            {
                return NotFound();
            }

            return lineitem;
        }

        // PUT: api/Lineitems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineitem(int id, Lineitem lineitem)
        {
            if (id != lineitem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineitem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineitemExists(id))
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

        // POST: api/Lineitems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lineitem>> PostLineitem(Lineitem lineitem)
        {
            _context.Lineitems.Add(lineitem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLineitem", new { id = lineitem.Id }, lineitem);
        }

        // DELETE: api/Lineitems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lineitem>> DeleteLineitem(int id)
        {
            var lineitem = await _context.Lineitems.FindAsync(id);
            if (lineitem == null)
            {
                return NotFound();
            }

            _context.Lineitems.Remove(lineitem);
            await _context.SaveChangesAsync();

            return lineitem;
        }

        private bool LineitemExists(int id)
        {
            return _context.Lineitems.Any(e => e.Id == id);
        }
    }
}
