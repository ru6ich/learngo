using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� MVC
builder.Services.AddControllersWithViews();

// ����������� ������ ����������� � ������� "����=��������"
var connectionString = "Server=mysql.railway.internal;Port=3306;Database=railway;User=root;Password=PiUAlNuyDnapmfySiZxvaQdrgLzjwBOA;";

// �������� ������ ����������� ��� �������
Console.WriteLine($"Using connection string: {connectionString}");

// ����������� ��������� �����������, ��������� ������� �������� SSL
var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString)
{
    SslMode = MySqlSslMode.Disabled // ��������� SSL ��� ������
};
connectionString = connectionStringBuilder.ToString();

// ����������� DbContext ��� ������������� MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));

// �������� ���� �� ���������� ��������� PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
Console.WriteLine($"Using port: {port}");

// ����������� ���������� ��� ������������� ���������� �����
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// ����������� �������� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// ����������� �������� ��� MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();