 using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository repository;
    public ProductsController(IProductRepository repository)
    {
        this.repository = repository;
    }
  
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
    {
        return Ok(await repository.GetProductsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> AddProduct(Product product)
    {

        repository.AddProduct(product);

        if (await repository.SaveChangesAsync())
            return CreatedAtAction("GetProductById", new { id = product.Id }, product);
        else
            return BadRequest("Failed to add product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update product");
        else
        {
            repository.UpdateProduct(product);

            if (await repository.SaveChangesAsync())
                return NoContent();
            else
                return BadRequest("Failed to update product");
        }
    }

    private bool ProductExists(int id)
    {
        return repository.ProductExistsAsync(id);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        Product? product = await repository.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();
        else
        {
            repository.DeleteProduct(product);

            if (await repository.SaveChangesAsync())
                return NoContent();
            else
                return BadRequest("Failed to delete product");
        }
    }


    
}
