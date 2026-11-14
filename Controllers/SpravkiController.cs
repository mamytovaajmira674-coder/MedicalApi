using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SpravkiController : ControllerBase
{
    private readonly MedicalDbContext _db;
    public SpravkiController(MedicalDbContext db) { _db = db; }

          [HttpGet]
    public async Task<ActionResult<List<Spravka>>> GetAll(int? patientId, decimal? minPrice, string? sort)
    {
        IQueryable<Spravka> query = _db.Spravki;

        if (patientId != null)
        {
            query = query.Where(s => s.PatientId == patientId);
        }

        if (minPrice != null)
        {
            query = query.Where(s => s.Price >= minPrice);
        }

        if (sort == "price")
        {
            query = query.OrderBy(s => s.Price);
        }
        else
        {
            query = query.OrderBy(s => s.Id);
        }

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Spravka>> GetById(int id)
    {
        var s = await _db.Spravki.FirstOrDefaultAsync(x => x.Id == id);
        return s == null ? NotFound() : s;
    }

    [HttpPost]
    [HttpPost]
public async Task<ActionResult<Spravka>> Create([FromBody] Spravka spravka)
    {
        spravka.Id = 0;
        if (!await _db.Patients.AnyAsync(p => p.Id == spravka.PatientId))
            return BadRequest("Пациент с таким ID не найден");
        if (!await _db.SpravkaTypes.AnyAsync(t => t.Id == spravka.SpravkaTypeId))
            return BadRequest("Вид справки с таким ID не найден");
        if (spravka.Price <= 0)
            return BadRequest("Цена должна быть больше 0");

        _db.Spravki.Add(spravka);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = spravka.Id }, spravka);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _db.Spravki.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return NotFound();
        _db.Spravki.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}