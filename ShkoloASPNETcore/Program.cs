using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Core.Services.Contracts;
using ShkoloASPNETcore.Infrastructure.Data;
using ShkoloASPNETcore.Infrastructure.Data.Models;
using ShkoloASPNETcore.Services;
using ShkoloASPNETcore.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ShkoloDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ShkoloDbContext>();

builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<IRemarkService, RemarkService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Errors/Error{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();