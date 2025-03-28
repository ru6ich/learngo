using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

// Получаем строку подключения из appsettings.json или переменной окружения DATABASE_URL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Настраиваем DbContext для использования PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Настраиваем конвейер обработки запросов
app.UseStaticFiles();
app.UseRouting();



// Настраиваем маршруты для MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();