using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class SpravkaTypesController : ControllerBase
{
    public static List<SpravkaType> Types = new List<SpravkaType>
    {
        new SpravkaType { Id = 1, Name = "Справка 083", Price = 100, ValidDays = 30, NeedDoctor = false, IsForChildren = false },
        new SpravkaType { Id = 2, Name = "Санитарная книжка", Price = 750, ValidDays = 365, NeedDoctor = true, IsForChildren = false },
        new SpravkaType { Id = 3, Name = "Справка в бассейн", Price = 200, ValidDays = 90, NeedDoctor = true, IsForChildren = true },
        new SpravkaType { Id = 4, Name = "Справка для водительских прав", Price = 500, ValidDays = 365, NeedDoctor = true, IsForChildren = false }
    };

    [HttpGet]
    public List<SpravkaType> GetAll() => Types;

    [HttpGet("{id}")]
    public ActionResult<SpravkaType> GetById(int id)
    {
        var t = Types.FirstOrDefault(x => x.Id == id);
        if (t == null) return NotFound();
        return t;
    }

    [HttpPost]
    public ActionResult<SpravkaType> Create(SpravkaType type)
    {
        if (string.IsNullOrWhiteSpace(type.Name)) return BadRequest("Название не может быть пустым");
        if (type.Price <= 0) return BadRequest("Цена должна быть больше 0");
        type.Id = Types.Any() ? Types.Max(s=>s.Id) +1 :1;
        Types.Add(type);
        return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
    }

    [HttpPut("{id}")]
    public ActionResult<SpravkaType> Update(int id, SpravkaType type)
    {
        var existing = Types.FirstOrDefault(t => t.Id == id);
        if (existing == null) return NotFound();
        if (string.IsNullOrWhiteSpace(type.Name)) return BadRequest("Название не может быть пустым");
        if (type.Price <= 0) return BadRequest("Цена должна быть больше 0");
        existing.Name = type.Name;
        existing.Price = type.Price;
        existing.ValidDays = type.ValidDays;
        existing.NeedDoctor = type.NeedDoctor;
        existing.IsForChildren = type.IsForChildren;
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var existing = Types.FirstOrDefault(t => t.Id == id);
        if (existing == null) return NotFound();
        Types.Remove(existing);
        return NoContent();
    }
}