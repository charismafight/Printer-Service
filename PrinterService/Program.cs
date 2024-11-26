using Scalar.AspNetCore;
using Microsoft.Extensions.FileProviders;
using PrinterService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<PrinterContext>(opt =>
    opt.UseSqlite("Data Source=" + Path.Combine(builder.Environment.WebRootPath, "Db", "printer.db") + ";Cache=Shared"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.MapScalarApiReference();
app.MapOpenApi();
// }

AppSetting.Init(app.Services.GetService<IConfiguration>());
StringExtension.InitPrintCommand(AppSetting.Config?.GetSection("PrintCommand").Value);

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

var staticFolder = builder.Configuration.GetSection("StaticFileFolder").Value ?? "StaticFiles";
var staticFullFolder = Path.Combine(builder.Environment.ContentRootPath, staticFolder);
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(staticFullFolder),
    RequestPath = $"/{staticFolder}",
    EnableDirectoryBrowsing = true
});
Folders.StaticFolder = staticFullFolder;

app.MapControllers();

app.Run();
