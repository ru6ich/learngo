using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� MVC
builder.Services.AddControllersWithViews();

// ����������� ������ ����������� ��� �������
var connectionString = "mysql://root:PiUAlNuyDnapmfySiZxvaQdrgLzjwBOA@mysql.railway.internal:3306/railway";
// ���� ������ ����������� ���������� ����� ������������ MySQL-�������, �������� � �� �����, ��������:
// var connectionString = "mysql://root:newpassword@mysql.railway.internal:3306/railway";

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