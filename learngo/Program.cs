using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� MVC
builder.Services.AddControllersWithViews();

// �������� ������ ����������� �� appsettings.json ��� ���������� ��������� DATABASE_URL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ����������� DbContext ��� ������������� PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// ����������� �������� ��������� ��������
app.UseStaticFiles();
app.UseRouting();



// ����������� �������� ��� MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();