namespace MedicalApi.Models;
public class Patient
{
    public int Id{get;set;}
    public string Name{get;set;} = "";
    public DateTime BirthDate{get;set;}
    public string PhoneNumber{get;set;} = ""; //поле телефон
    public bool IsMale{get;set;}//true- woman, false-man
    public string Address{get;set;} = ""; //адрес прописки
}