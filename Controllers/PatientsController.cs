using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public PatientsController(MedicalDbContext db) { _db = db; }

    [HttpGet]
    public async Task<List<Patient>> GetAll() => await _db.Patients.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        var p = await _db.Patients.FirstOrDefaultAsync(x => x.Id == id);
        return p == null ? NotFound() : p;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        var error = Validate(patient);
        if (error != null) return BadRequest(error);
        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Patient>> Update(int id, Patient patient)
    {
        var existing = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null) return NotFound();
        var error = Validate(patient);
        if (error != null) return BadRequest(error);

        existing.Name = patient.Name;
        existing.BirthDate = patient.BirthDate;
        existing.PhoneNumber = patient.PhoneNumber;
        existing.IsMale = patient.IsMale;
        existing.Address = patient.Address;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null) return NotFound();
        _db.Patients.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private string? Validate(Patient p)
    {
        if (string.IsNullOrWhiteSpace(p.Name)) return "Имя не может быть пустым";
        if (p.BirthDate > DateOnly.FromDateTime(DateTime.Now)) return "Дата рождения не может быть в будущем";
        return null;
    }
}