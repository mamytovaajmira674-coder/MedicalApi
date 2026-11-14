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

    // 1. GET: поиск пациентов
    [HttpGet]
    public async Task<List<Patient>> GetAll(string? name)
    {
        IQueryable<Patient> query = _db.Patients;
        
        if (name != null)
        {
            query = query.Where(p => EF.Functions.ILike(p.Name, $"%{name}%"));
        }
        
        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    // 2. GET: получить одного пациента по Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(int id)
    {
        var p = await _db.Patients.FirstOrDefaultAsync(x => x.Id == id);
        return p == null ? NotFound() : p;
    }

    // 3. GET: получить все справки пациента
    [HttpGet("{id}/spravki")]
    public async Task<ActionResult<List<Spravka>>> GetSpravki(int id)
    {
        var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null) return NotFound("Пациент не найден");

        var spravki = await _db.Spravki.Where(s => s.PatientId == id).ToListAsync();
        return spravki;
    }

    // 4. POST: создать пациента
    [HttpPost]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        var error = Validate(patient);
        if (error != null) return BadRequest(error);

        patient.Id = 0; // добвленная строка задание 9 баги
        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    // 5. PUT: обновить пациента
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
        existing.Email = patient.Email;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    // 6. DELETE: удалить пациента (с проверкой на справки)
    [HttpDelete("{id}")]
    //ПРОверка: есть ли у пациента справки?
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null) return NotFound();

        // ПРОВЕРКА: есть ли у пациента справки?
        var hasSpravki = await _db.Spravki.AnyAsync(s => s.PatientId == id);
        if (hasSpravki)
        {
            return BadRequest("Нельзя удалить пациента, у него есть справки");
        }

        _db.Patients.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // 7. GET: статистика
    [HttpGet("/api/stats")]
    public async Task<ActionResult> GetStats()
    {
        var patientCount = await _db.Patients.CountAsync();
        var spravkaCount = await _db.Spravki.CountAsync();
        var totalSum = await _db.Spravki.SumAsync(s => s.Price);

        return Ok(new
        {
            Patients = patientCount,
            Spravki = spravkaCount,
            TotalSum = totalSum
        });
    }

    // Метод проверки
    private string? Validate(Patient p)
    {
        if (string.IsNullOrWhiteSpace(p.Name)) return "Имя не может быть пустым";
        if (p.BirthDate > DateOnly.FromDateTime(DateTime.Now)) return "Дата рождения не может быть в будущем";
        return null;
    }
}