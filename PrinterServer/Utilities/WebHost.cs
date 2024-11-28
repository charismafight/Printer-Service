using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace PrinterServer.Utilities
{
    public static class WebHost
    {
        public static void StartPrintServer()
        {
            // start webapi
            var app = WebApplication.Create();
            app.Urls.Add("http://0.0.0.0:5000");
            // just for test
            app.MapGet("/Print", () => "");
            app.MapPost("/Print", async (IFormFile? attachment) =>
            {
                if (attachment is not null)
                {
                    using var stream = attachment.OpenReadStream();

                    // remove files in temp folder
                    try
                    {
                        Directory.GetFiles("temp").ToList().ForEach(File.Delete);
                    }
                    catch (Exception)
                    {
                    }

                    return Results.Ok();
                }

                return Results.NotFound();
            }).DisableAntiforgery();

            app.MapPost("/Upload", async (IFormFile? attachment) =>
            {
                if (attachment is not null)
                {
                    using var stream = attachment.OpenReadStream();

                    // remove files in temp folder
                    try
                    {
                        Directory.GetFiles("temp").ToList().ForEach(File.Delete);
                    }
                    catch (Exception)
                    {
                    }

                    return Results.Ok();
                }

                return Results.NotFound();
            }).DisableAntiforgery();
            app.RunAsync();
        }
    }
}
