using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Program;

namespace Management.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: api/products/{id}
        // GET: api/products/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.productsModel.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products

        [Authorize]
        [HttpPost("api/products")] 
        public async Task<IActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = _userManager.GetUserId(User);
            product.ManufactureEmail = loggedInUserId;

            _context.productsModel.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }




        private object GetProduct()
        {
            throw new NotImplementedException();
        }

        // PUT: api/products/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _context.productsModel.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var loggedInUserId = _userManager.GetUserId(User);
            if (existingProduct.ManufactureEmail != loggedInUserId)
            {
                return Forbid();
            }

            existingProduct.Name = product.Name;
            existingProduct.IsAvailable = product.IsAvailable;
            existingProduct.ManufacturePhone = product.ManufacturePhone;
            existingProduct.ProduceDate = product.ProduceDate;
            

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.productsModel.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var loggedInUserId = _userManager.GetUserId(User);
            if (product.ManufactureEmail != loggedInUserId)
            {
                return Forbid();
            }

            _context.productsModel.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/products/filter
        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByManufacturer(string manufacturerEmail)
        {
            var products = await _context.productsModel
       .Where(p => p.ManufactureEmail == manufacturerEmail)
       .ToListAsync();
            return Ok(products);
        }

    }
}
