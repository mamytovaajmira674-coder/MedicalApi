using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private static List<Appointment> _appointments = new List<Appointment>();

    [HttpGet]
    public List<Appointment> GetAll() => _appointments;

    [HttpGet("{id}")]
    public ActionResult<Appointment> GetById(int id)
    {
        var a = _appointments.FirstOrDefault(x => x.Id == id);
        if (a == null) return NotFound();
        return a;
    }

    [HttpPost]
    public ActionResult<Appointment> Create(Appointment appointment)
    {
        // ДОБАВЛЕНО: проверки данных
        if (appointment.PatientId <= 0)
            return BadRequest("ID пациента должен быть больше 0");
        if (string.IsNullOrWhiteSpace(appointment.DoctorName))
            return BadRequest("Имя врача не может быть пустым");
        if (appointment.AppointmentDate < DateTime.Now)
            return BadRequest("Дата записи не может быть в прошлом");

        // ДОБАВЛЕНО: проверка, что пациент существует
        var patientExists = PatientsController.Patients.Any(p => p.Id == appointment.PatientId);
        if (!patientExists)
            return BadRequest("Пациент с таким ID не найден");

        appointment.Id = _appointments.Any() ? _appointments.Max(a=> a.Id) +1 : 1;
        _appointments.Add(appointment);
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public ActionResult<Appointment> Update(int id, Appointment appointment)
    {
        var existing = _appointments.FirstOrDefault(a => a.Id == id);
        if (existing == null) return NotFound();

        if (appointment.PatientId <= 0)
            return BadRequest("ID пациента должен быть больше 0");
        if (string.IsNullOrWhiteSpace(appointment.DoctorName))
            return BadRequest("Имя врача не может быть пустым");
        if (appointment.AppointmentDate < DateTime.Now)
            return BadRequest("Дата записи не может быть в прошлом");

        var patientExists = PatientsController.Patients.Any(p => p.Id == appointment.PatientId);
        if (!patientExists)
            return BadRequest("Пациент с таким ID не найден");

        existing.PatientId = appointment.PatientId;
        existing.DoctorName = appointment.DoctorName;
        existing.AppointmentDate = appointment.AppointmentDate;
        existing.Status = appointment.Status;

        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = _appointments.FirstOrDefault(a => a.Id == id);
        if (existing == null) return NotFound();
        _appointments.Remove(existing);
        return NoContent();
    }
}