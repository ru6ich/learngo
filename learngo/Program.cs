using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

// Получаем строку подключения из переменной окружения MYSQL_URL (Railway) или appsettings.json
var connectionString = Environment.GetEnvironmentVariable("MYSQL_URL") ??
    builder.Configuration.GetConnectionString("DefaultConnection");

// Настраиваем DbContext для использования MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Применяем миграции при запуске приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Настраиваем конвейер обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// Настраиваем маршруты для MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();