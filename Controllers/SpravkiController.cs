using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class SpravkiController : ControllerBase
{
    public static List<Spravka> _spravki = new List<Spravka>();

    [HttpGet]
    public List<Spravka> GetAl(int? patientId = null)
    {
        if (patientId == null)
        {
            return _spravki;// Если ID не передан, возвращаем все справки
        }
        return _spravki.Where(s=> s.PatientId == patientId).ToList(); // Иначе — только справки этого пациента

    }

    [HttpGet("{id}")]
    public ActionResult<Spravka> GetById(int id)
    {
        var s = _spravki.FirstOrDefault(x => x.Id == id);
        if (s == null) return NotFound();
        return s;
    }

    [HttpPost]
    public ActionResult<Spravka> Create(Spravka spravka)
    {
        var error = Validate(spravka);
        if (error != null) return BadRequest(error);

        // Проверка, что пациент существует
        var patientExists = PatientsController.Patients.Any(p => p.Id == spravka.PatientId);
        if (!patientExists) return BadRequest("Пациент с таким ID не найден");

        // ДОБАВЛЕНО: ищем вид в каталоге
        var type = SpravkaTypesController.Types.FirstOrDefault(t => t.Id == spravka.SpravkaTypeId);
        if (type == null) return BadRequest("Вид справки с таким ID не найден");

        // ДОБАВЛЕНО: название и цену берем из каталога
        spravka.Name = type.Name;
        spravka.Price = type.Price;

        spravka.Id = _spravki.Any() ? _spravki.Max(s=> s.Id) + 1:1;
        _spravki.Add(spravka);
        return CreatedAtAction(nameof(GetById), new { id = spravka.Id }, spravka);
    }

    [HttpPut("{id}")]
    public ActionResult<Spravka> Update(int id, Spravka spravka)
    {
        var existing = _spravki.FirstOrDefault(s => s.Id == id);
        if (existing == null) return NotFound();

        var error = Validate(spravka);
        if (error != null) return BadRequest(error);

        var patientExists = PatientsController.Patients.Any(p => p.Id == spravka.PatientId);
        if (!patientExists) return BadRequest("Пациент с таким ID не найден");

        var type = SpravkaTypesController.Types.FirstOrDefault(t => t.Id == spravka.SpravkaTypeId);
        if (type == null) return BadRequest("Вид справки с таким ID не найден");

        existing.Name = type.Name;
        existing.Price = type.Price;
        existing.PatientId = spravka.PatientId;
        existing.SpravkaTypeId = spravka.SpravkaTypeId;

        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = _spravki.FirstOrDefault(s => s.Id == id);
        if (existing == null) return NotFound();
        _spravki.Remove(existing);
        return NoContent();
    }

    private string? Validate(Spravka spravka)
    {
        // Проверку названия и цены убрали, так как они берутся из каталога
        if (spravka.PatientId <= 0) return "ID пациента должен быть больше 0";
        if (spravka.SpravkaTypeId <= 0) return "ID вида справки должен быть больше 0";
        return null;
    }
        [HttpGet("stats")]
    public ActionResult<object> GetStats()
    {
        var totalCount = _spravki.Count; // Сколько всего справок
        var totalSum = _spravki.Sum(s => s.Price); // На какую сумму

        return Ok(new
        {
            TotalSpravki = totalCount,
            TotalPrice = totalSum
        });
    }
}