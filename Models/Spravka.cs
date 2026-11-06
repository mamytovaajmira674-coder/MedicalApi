namespace MedicalApi.Models;
public class Spravka
{
    public int Id{get;set;}
    public string Name{get;set;} = "";
    public int PatientId{get;set;} // ДОБАВЛЕНО: ID пациента вместо имени
    public int SpravkaTypeId{get;set;} // ДОБАВЛЕНО: ID вида справки
    public decimal Price{get;set;}
}