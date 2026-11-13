using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;
using MedicalApi.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SpravkaTypeController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public SpravkaTypeController(MedicalDbContext db)
    {
        _db = db;
    }

    // GET: api/SpravkaType
    [HttpGet]
    public async Task<ActionResult<List<SpravkaType>>> GetAll()
    {
        return await _db.SpravkaTypes.ToListAsync();
    }

    // GET: api/SpravkaType/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SpravkaType>> GetById(int id)
    {
        var type = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (type == null) return NotFound();
        return type;
    }

    // POST: api/SpravkaType
    [HttpPost]
    public async Task<ActionResult<SpravkaType>> Create(SpravkaType spravkaType)
    {
        // Проверка: цена должна быть больше 0 (как в SQL-ограничении)
        if (spravkaType.Price <= 0)
            return BadRequest("Цена должна быть больше 0");

        _db.SpravkaTypes.Add(spravkaType);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = spravkaType.Id }, spravkaType);
    }

    // PUT: api/SpravkaType/5
    [HttpPut("{id}")]
    public async Task<ActionResult<SpravkaType>> Update(int id, SpravkaType spravkaType)
    {
        var existing = await _db.SpravkaTypes.FirstOrDefaultAsync(t => t.Id == id);
        if (existing == null) return NotFound();

        if (spravkaType.Price <= 0)
            return BadRequest("Цена должна быть больше 0");

        existing.Name = spravkaType.Name;
        existing.Price = spravkaType.Price;
        existing.ValidDays = spravkaType.ValidDays;
        
        // Если у вас есть поле NeedDoctor, раскомментируйте строчку ниже:
        // existing.NeedDoctor = spravkaType.NeedDoctor;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    // DELETE: api/SpravkaType/5
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