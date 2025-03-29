using Backend.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

// Используем публичный URL Railway
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

// Остальной код остаётся без изменений
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Console.WriteLine($"Using port: {port}");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();