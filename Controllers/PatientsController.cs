using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private static List<Patient> _patients = new List<Patient>();

    [HttpGet]
    public List<Patient> GetAll() => _patients;

    [HttpGet("{id}")]
    public ActionResult<Patient> GetById(int id)
    {
        var p = _patients.FirstOrDefault(x => x.Id == id);
        return p == null ? NotFound() : p;
    }

    [HttpPost]
    public ActionResult<Patient> Create(Patient patient)
    {
        patient.Id = _patients.Count + 1;
        _patients.Add(patient);
        return Ok(patient);
    }
}