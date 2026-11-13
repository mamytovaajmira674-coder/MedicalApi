using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SpravkaTypesController : ControllerBase
{
    private readonly MedicalDbContext _db;
    public SpravkaTypesController(MedicalDbContext db) { _db = db; }

    [HttpGet]
    public async Task<ActionResult<List<SpravkaType>>> GetAll()
    {
        return await _db.SpravkaTypes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpravkaType>> GetById(int id)
    {
        var t = await _db.SpravkaTypes.FirstOrDefaultAsync(x => x.Id == id);
        return t == null ? NotFound() : t;
    }

    [HttpPost]
    public async Task<ActionResult<SpravkaType>> Create(SpravkaType type)
    {
        if (string.IsNullOrWhiteSpace(type.Name)) return BadRequest("Название не может быть пустым");
        if (type.Price <= 0) return BadRequest("Цена должна быть больше 0");
        _db.SpravkaTypes.Add(type);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SpravkaType>> Update(int id, SpravkaType type)
    {
        var existing = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null) return NotFound();
        if (string.IsNullOrWhiteSpace(type.Name)) return BadRequest("Название не может быть пустым");
        if (type.Price <= 0) return BadRequest("Цена должна быть больше 0");

        existing.Name = type.Name;
        existing.Price = type.Price;
        existing.ValidDays = type.ValidDays;
        existing.NeedDoctor = type.NeedDoctor;
        existing.IsForChildren = type.IsForChildren;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null) return NotFound();
        _db.SpravkaTypes.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}