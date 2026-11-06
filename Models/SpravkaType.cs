namespace MedicalApi.Models;
public class SpravkaType
{
    public int Id{get;set;}
    public string Name{get;set;} = "";
    public decimal Price{get;set;}
    public int ValidDays{get;set;} = 30;// МОЕ ПОЛЕ: сколько дней действует
    public bool NeedDoctor {get;set;} = false;// МОЕ ПОЛЕ: нужен ли осмотр врача
    public bool IsForChildren{get;set;} = false;//МОЕ ПОЛЕ: для детей (true) или для взрослых (false)

}