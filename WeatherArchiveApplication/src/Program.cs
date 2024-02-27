using WeatherArchiveApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IMonthExtractor<string>, MonthStringExtractor>()
                .AddTransient<IYearExtractor<string>, YearStringExtractor>()
                .AddTransient<IPartOfRecordsExtractor, PartOfRecordsExtractor>()
                .AddTransient<IDateParamsValidator<string>, DateStringParamsValidator>()
                .AddTransient<IMonthSorter<string>, MonthStringSorter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.Migrate();
    db.SaveChanges();
}

app.Run();
