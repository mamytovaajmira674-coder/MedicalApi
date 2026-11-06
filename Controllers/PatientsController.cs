using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    public static List<Patient> Patients = new List<Patient>();

    [HttpGet]
    public List<Patient> GetAll()
    {
        return Patients;
    }

    [HttpGet("{id}")]
    public ActionResult<Patient> GetById(int id)
    {
        var p = Patients.FirstOrDefault(x => x.Id == id);
        if (p == null) return NotFound();
        return p;
    }

    [HttpPost]
    public ActionResult<Patient> Create(Patient patient)
    {
        // ДОБАВЛЕНО: вызов общей проверки
        var error = Validate(patient);
        if (error != null) return BadRequest(error);

        patient.Id = Patients.Any() ? Patients.Max(s=> s.Id) + 1:1;
        Patients.Add(patient);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public ActionResult<Patient> Update(int id, Patient patient)
    {
        var existing = Patients.FirstOrDefault(p => p.Id == id);
        if (existing == null) return NotFound();

        // ДОБАВЛЕНО: вызов общей проверки
        var error = Validate(patient);
        if (error != null) return BadRequest(error);

        existing.Name = patient.Name;
        existing.BirthDate = patient.BirthDate;
        existing.PhoneNumber = patient.PhoneNumber;
        existing.IsMale = patient.IsMale;
        existing.Address = patient.Address;

        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = Patients.FirstOrDefault(p => p.Id == id);
        if (existing == null) return NotFound();
        Patients.Remove(existing);
        return NoContent();
    }

    [HttpGet("{id}/spravki")]
    public ActionResult<List<Spravka>> GetSpravki(int id)
    {
        var patient = Patients.FirstOrDefault(p => p.Id == id);
        if (patient == null) return NotFound("Пациент не найден");

        var spravki = SpravkiController._spravki.Where(s => s.PatientId == id).ToList();
        return spravki;
    }

    // ДОБАВЛЕНО: приватный метод для проверок
    private string? Validate(Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.Name))
            return "Имя пациента не может быть пустым";
        if (patient.BirthDate > DateTime.Now)
            return "Дата рождения не может быть в будущем";
        return null;
    }
}