using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(); //ДОБАВЛЕНО: разрешаем запросы с других адресов (для test.html)
builder.Services.AddOpenApi(); // Генератор из методички

var app = builder.Build();
app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());//добавили

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();//ДОБАВЛЕНО: генератор OpenAPI
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "v1"));// ДОБАВЛЕНО: подключение Swagger UI

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();