using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

// Захардкодим строку подключения в формате "ключ=значение"
var connectionString = "Server=mysql.railway.internal;Port=3306;Database=railway;User=root;Password=PiUAlNuyDnapmfySiZxvaQdrgLzjwBOA;";

// Логируем строку подключения для отладки
Console.WriteLine($"Using connection string: {connectionString}");

// Настраиваем параметры подключения, отключаем строгую проверку SSL
var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString)
{
    SslMode = MySqlSslMode.Disabled // Отключаем SSL для тестов
};
connectionString = connectionStringBuilder.ToString();

// Настраиваем DbContext для использования MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));

// Получаем порт из переменной окружения PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Console.WriteLine($"Using port: {port}");

// Настраиваем приложение для прослушивания указанного порта
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

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