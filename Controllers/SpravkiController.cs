using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SpravkiController : ControllerBase
{
    private static List<Spravka> _spravki = new List<Spravka>();

    [HttpGet]
    public List<Spravka> GetAll()
    {
        return _spravki;
    }

    [HttpPost]
    public ActionResult<Spravka> Create(Spravka spravka)
    {
        spravka.Id = _spravki.Count + 1;
        _spravki.Add(spravka);
        return Ok(spravka);
    }
}