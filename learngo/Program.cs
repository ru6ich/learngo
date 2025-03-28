using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� MVC
builder.Services.AddControllersWithViews();

// �������� ������ ����������� �� ���������� ��������� MYSQL_URL (Railway) ��� appsettings.json
var connectionString = Environment.GetEnvironmentVariable("MYSQL_URL") ??
    builder.Configuration.GetConnectionString("DefaultConnection");

// ����������� DbContext ��� ������������� MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// ��������� �������� ��� ������� ����������
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

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