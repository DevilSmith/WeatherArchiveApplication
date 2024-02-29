using WeatherArchiveApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(option =>
    {
        option.MultipartBodyLengthLimit = 512 * 1024 * 1024;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IMonthExtractor<string>, MonthStringExtractor>()
                .AddTransient<IYearExtractor<string>, YearStringExtractor>()
                .AddTransient<IPartOfRecordsExtractor, PartOfRecordsExtractor>()
                .AddTransient<IDateParamsValidator<string>, DateStringParamsValidator>()
                .AddTransient<IMonthSorter<string>, MonthStringSorter>()
                .AddTransient<IFileExtensionValidator<IFormFile, bool>, ExcelFileExtensionValidator>()
                .AddTransient<IFileDataValidator<IFormFile>, ExcelFileModelValidator>()
                .AddTransient<IDataExtractor<List<WeatherRecord>, IFormFile>, ExcelDataExtractor>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (ApplicationContext db = new ApplicationContext())
{
    db.Database.Migrate();
    db.SaveChanges();
}

app.Run();
