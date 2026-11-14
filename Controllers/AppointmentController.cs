using MedicalApi.Data;
using MedicalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly MedicalDbContext _db;
    public AppointmentsController(MedicalDbContext db) { _db = db; }

    [HttpGet]
    public async Task<ActionResult<List<Appointment>>> GetAll()
    {
        return await _db.Appointments.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetById(int id)
    {
        var a = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id);
        return a == null ? NotFound() : a;
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> Create(Appointment appointment)
    {
        appointment.Id = 0;
        if (appointment.PatientId <= 0) return BadRequest("ID пациента должен быть больше 0");
        if (string.IsNullOrWhiteSpace(appointment.DoctorName)) return BadRequest("Имя врача не может быть пустым");
        if (appointment.AppointmentDate < DateTime.Now) return BadRequest("Дата записи не может быть в прошлом");

        if (!await _db.Patients.AnyAsync(p => p.Id == appointment.PatientId))
            return BadRequest("Пациент с таким ID не найден");

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Appointment>> Update(int id, Appointment appointment)
    {
        var existing = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        if (existing == null) return NotFound();

        if (appointment.PatientId <= 0) return BadRequest("ID пациента должен быть больше 0");
        if (string.IsNullOrWhiteSpace(appointment.DoctorName)) return BadRequest("Имя врача не может быть пустым");
        if (appointment.AppointmentDate < DateTime.Now) return BadRequest("Дата записи не может быть в прошлом");

        if (!await _db.Patients.AnyAsync(p => p.Id == appointment.PatientId))
            return BadRequest("Пациент с таким ID не найден");

        existing.PatientId = appointment.PatientId;
        existing.DoctorName = appointment.DoctorName;
        existing.AppointmentDate = appointment.AppointmentDate;
        existing.Status = appointment.Status;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var existing = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        if (existing == null) return NotFound();
        _db.Appointments.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}