using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

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
            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.MapScalarApiReference();
            app.MapOpenApi();
            app.UseCors("AllowAll");
            var url = "http://0.0.0.0:9991";
            app.Urls.Add(url);
            // just for test
            app.MapGet("/Print", () => "");
            app.MapPost("/Print", async (IFormFile? files) =>
            {
                if (files is not null)
                {
                    var filePath = await SaveFileAsync(files);
                    LogViewModel.Instance.L($"{files.Name} saved at {filePath}");
                    filePath.Print();
                    LogViewModel.Instance.L($"{files.Name} printed");
                    return Results.Ok();
                }

                LogViewModel.Instance.L("Upload file is null");
                return Results.NotFound();
            }).DisableAntiforgery()
            .RequireCors();

            app.MapPost("/Upload", async (IFormFile? files) =>
            {
                if (files is not null)
                {
                    var filePath = await SaveFileAsync(files);
                    LogViewModel.Instance.L($"{files.Name} saved at {filePath}");
                    return Results.Ok();
                }

                LogViewModel.Instance.L("Upload file is null");
                return Results.NotFound();
            }).DisableAntiforgery();
            app.RunAsync();
            LogViewModel.Instance.L($"Print server started, listening on {url}");
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
