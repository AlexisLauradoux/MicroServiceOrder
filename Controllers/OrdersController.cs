using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroServiceOrder;
using EntrepotService;
using System.Net.Http;

namespace MicroServiceOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EntrepotClient _entrepotClient;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;

            // Configure HttpClientHandler to ignore certificate validation errors.
             var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            // Create WeatherClient.
             var httpClient = new HttpClient(httpClientHandler);
            _entrepotClient = new EntrepotClient("http://93.11.120.71:3769/", httpClient);
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.Items).ToListAsync();
            foreach (var item in orders)
            {
                foreach (var item1 in item.Items)
                {
                    item1.Product = await _entrepotClient.Products2Async(item1.ProductId);
                }
            }
            return orders;
        }

        // GET: api/Orders/NotDone
        [HttpGet("NotDone")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersNotDone()
        {
            var orders = await _context.Orders.Include(o => o.Items).Where(v => v.Done == false).ToListAsync();
            foreach (var item in orders)
            {
                foreach (var item1 in item.Items)
                {
                    item1.Product = await _entrepotClient.Products2Async(item1.ProductId);
                }
            }
            return orders; 
        }

        // GET: api/Orders/Done
        [HttpGet("Done")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersDone()
        {
            var orders = await _context.Orders.Include(o => o.Items).Where(v => v.Done == true).ToListAsync();
            foreach (var item in orders)
            {
                foreach (var item1 in item.Items)
                {
                    item1.Product = await _entrepotClient.Products2Async(item1.ProductId);
                }
            }
            return orders;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _context.Orders.Include(o => o.Items).Where(o => o.Id == id).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(string id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
