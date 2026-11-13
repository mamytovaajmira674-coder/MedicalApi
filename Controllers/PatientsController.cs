using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly MedicalDbContext _db;

    public PatientsController(MedicalDbContext db)
    {
        _db = db;
    }

    // 1. GET: получить всех пациентов
    [HttpGet]
    public async Task<List<Patient>> GetAll()
    {
        return await _db.Patients.ToListAsync();
    }

    // 2. GET: получить одного пациента по Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        Patient? patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null)
        {
            return NotFound();
        }
        return patient;
    }

    // 3. POST: создать пациента
    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        var error = Validate(patient);
        if (error != null) return BadRequest(error);

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    // 4. PUT: обновить пациента (убрали [FromBody], чтобы Swagger не ругался)
    [HttpPut("{id}")]
           [HttpPut]
    public async Task<ActionResult<Patient>> Update(Patient patient)
    {
        var existing = await _db.Patients.FirstOrDefaultAsync(p => p.Id == patient.Id);
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

    // 5. DELETE: удалить пациента
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null) return NotFound();

        _db.Patients.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Метод проверки
    private string? Validate(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.Name))
            return "Имя пациента не может быть пустым";
            
        if (patient.BirthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return "Дата рождения не может быть в будущем";
        }
        
        return null;
    }
}