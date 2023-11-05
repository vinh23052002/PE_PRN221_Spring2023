using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PRN_Spr23_B1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection") ?? throw new InvalidOperationException("Connection string 'DBContext_28_NguyenQuangVinh' not found.")));
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();
app.MapRazorPages();
app.UseHttpsRedirection();
app.UseRouting();

app.MapGet("/", () => "Hello World!");
app.MapHub<EmployeeHub>("/employeeHub");
app.Run();
