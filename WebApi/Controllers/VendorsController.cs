using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;
using WebApi.Services.Abstractions;

namespace WebApi.Controllers;

[Route("api/vendors")]
public class VendorsController : ControllerBase
{
    private readonly IVendorLoader _loader;

    public VendorsController(IVendorLoader loader)
    {
        _loader = loader;
    }
    
    // GET /api/vendors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorDto>>> GetAll()
    {
        var vendors = await _loader.GetAllAsync();
        return Ok(vendors);
    }

    // GET /api/vendors/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VendorDto>> GetById(string id)
    {
        var vendor = await _loader.GetByIdAsync(id);
        return Ok(vendor);
    }

    // POST /api/vendors
    [HttpPost]
    public async Task<ActionResult<VendorDto>> Create([FromBody] CreateVendorRequest newVendor)
    {
        var created = await _loader.CreateAsync(newVendor);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/vendors/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] VendorDto updatedVendor)
    {
        if (id != updatedVendor.Id)
            return BadRequest("URL id must match payload id.");

        await _loader.UpdateAsync(updatedVendor);
        
        return NoContent();
    }

    // DELETE /api/vendors/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _loader.DeleteAsync(id);
        return NoContent();
    }
}