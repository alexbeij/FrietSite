using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrietSite.Data;
using FrietSite.Models;  

namespace FrietprojectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Db _context;

        public ProductsController(Db context)
        {
            _context = context;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product met ID {id} niet gevonden." });
            }

            return Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] string name, [FromForm] decimal price, [FromForm] int categoryId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { Message = "De naam van het product is verplicht." });
            }

            var product = new Product
            {
                Name = name,
                Price = price,
                CategoryId = categoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] string name, [FromForm] decimal price, [FromForm] int categoryId)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product met ID {id} niet gevonden." });
            }

            product.Name = name;
            product.Price = price;
            product.CategoryId = categoryId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { Message = "Er is iets fout gegaan tijdens het bijwerken van het product." });
            }

            return NoContent();  
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product met ID {id} niet gevonden." });
            }

            _context.Products.Remove(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { Message = "Er is iets fout gegaan tijdens het verwijderen van het product." });
            }

            return NoContent();  
        }
    }
}
