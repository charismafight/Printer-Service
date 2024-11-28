using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PrinterServer.Utilities
{
    public static class WebHost
    {
        static string[] availableExtensions = [".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".pdf"];
        static string fileFolder = Path.Combine(Environment.CurrentDirectory, "UploadFiles");

        static WebHost()
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
        }

        public static void StartPrintServer()
        {
            // start web app
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            app.Urls.Add("http://0.0.0.0:9991");
            // just for test
            app.MapGet("/Print", () => "");
            app.MapPost("/Print", async (IFormFile? files) =>
            {
                if (files is not null)
                {
                    (await SaveFileAsync(files)).Print();

                    return Results.Ok();
                }

                return Results.NotFound();
            }).DisableAntiforgery()
            .RequireCors();

            app.MapPost("/Upload", async (IFormFile? files) =>
            {
                if (files is not null)
                {
                    await SaveFileAsync(files);
                    return Results.Ok();
                }

                return Results.NotFound();
            }).DisableAntiforgery();
            app.RunAsync();
        }

        static async Task<string> SaveFileAsync(IFormFile f)
        {
            var tempFileName = Guid.NewGuid().ToString() + "_" + f.FileName;
            var tempFileFullName = Path.Combine(fileFolder, tempFileName);
            using var fs = File.Create(tempFileFullName);
            var fileStream = f.OpenReadStream();
            _ = fileStream.Seek(0, SeekOrigin.Begin);
            await fileStream.CopyToAsync(fs).ConfigureAwait(false);
            fs.Close();
            var extension = Path.GetExtension(tempFileFullName);
            if (!availableExtensions.Contains(extension))
            {
                File.Delete(tempFileFullName);
                return string.Empty;
            }

            return tempFileFullName;
        }
    }
}
