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

        // TR4 #1
        [HttpGet("lines-for-pr/{id}")]
        public async Task<ActionResult<IEnumerable<Lineitem>>> GetAllByRequest(int id) {
            return await _context.Lineitems.Where(li => li.RequestId == id).ToListAsync();
        }

        
        // TR6 w/ Update
        // PUT: api/Lineitems/5
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
                await RecalculateTotal(lineitem.RequestId);
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

        // TR5 Recalculate w/ Add
        // POST: api/Lineitems
        [HttpPost]
        public async Task<ActionResult<Lineitem>> PostLineitem(Lineitem lineitem)
        {
            _context.Lineitems.Add(lineitem);
            await _context.SaveChangesAsync();
            await RecalculateTotal(lineitem.RequestId);

            return CreatedAtAction("GetLineitem", new { id = lineitem.Id }, lineitem);
        }

        // TR7
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

        // Recalculate Method
        private async Task RecalculateTotal(int requestid) {
            var request = await _context.Requests.FindAsync(requestid);
            request.Total = (from l in _context.Lineitems
                             join p in _context.Products on l.ProductId equals p.Id
                             where l.RequestId == requestid
                             select new { Total = l.Quantity * p.Price })
                             .Sum(x => x.Total);
            var rc = await _context.SaveChangesAsync();
            if (rc != 1) throw new Exception("Error Updating Total");

        }


    }
}
