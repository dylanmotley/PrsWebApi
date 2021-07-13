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
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // TR4 #2
        // GET: api/Requests/5
        [HttpGet("lines-for-pr/{id}")]
        public async Task<ActionResult<Request>> GetAllByRequest(int id) {
            var request = await _context.Requests
                                            .Include(r => r.User)
                                            .Include(r => r.Lineitems)
                                            .ThenInclude(rl => rl.Product)
                                            .SingleOrDefaultAsync(r => r.Id == id);
            if (request == null) {
                return NotFound();
            }

            return request;
        }

        // TR9 List Request Without Reviewer Listed
        [HttpGet("list-review/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetAllRequestBut(int id) {
            return await _context.Requests.Where(r => r.Id != id).ToListAsync();
        }

        // PUT: api/Requests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // TR3 Set Status to New
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            request.Status = "New";
            request.SubmittedDate = DateTime.Now;
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // POST: api/Requests
        // TR8 Set Status to Review or Approved
        [HttpPost("submit-review")]
        public async Task<ActionResult<Request>> UpdateRequestToReview(Request request) {
            if(request.Total <= 50) {
            request.Status = "Approved";
            } else {
                request.Status = "Review";
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }
        
        // TR13 Set Status to Approved
        [HttpPost("approve")]
        public async Task<ActionResult<Request>> UpdateRequestToApproved(Request request) {
                request.Status = "Approved";
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        
        // TR8 Set Status to Rejected
        [HttpPost("reject")]
        public async Task<ActionResult<Request>> UpdateRequestToRejected(Request request) {
                request.Status = "Rejected";
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }


        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }


    }
}
