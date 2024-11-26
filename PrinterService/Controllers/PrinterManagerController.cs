using AngleSharp.Text;
using Masuit.Tools;
using Masuit.Tools.Files.FileDetector;
using Microsoft.AspNetCore.Mvc;
using PrinterService.Models;

namespace PrinterService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PrinterManagerController(PrinterContext printerContext) : ControllerBase
{
    PrinterContext _printerContext = printerContext;

    string[] availableExtensions = [".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".pdf"];

    [HttpPost(Name = "Print")]
    [ActionName("Print")]
    public async Task<IActionResult> Print(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest();
        }

        await files.ForeachAsync(async f =>
        {
            var fileFullName = await SaveFileAsync(f);
            // execute printer command to print document
            if (string.IsNullOrWhiteSpace(fileFullName))
            {
                return;
            }

            fileFullName.Print();
            _printerContext.Add(new PrintRecord
            {
                FileLocation = fileFullName,
                IP = GetIPAddress(),
                FileName = f.FileName,
                PrintTime = DateTime.Now
            });
            await _printerContext.SaveChangesAsync();
        });

        return Ok();
    }

    protected string? GetIPAddress()
    {
        var ipAddress = Request.Headers["HTTP_X_FORWARDED_FOR"].ToString();

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
            {
                return addresses[0];
            }
        }

        return ipAddress;
    }

    [HttpPost(Name = "Upload")]
    [ActionName("Upload")]
    public async Task<IActionResult> Upload(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest();
        }

        await files.ForeachAsync(SaveFileAsync);
        return Ok();
    }

    async Task<string> SaveFileAsync(IFormFile f)
    {
        var tempFileName = Guid.NewGuid().ToString() + "_" + f.FileName;
        var tempFileFullName = Path.Combine(Folders.StaticFolder, tempFileName);
        using var fs = System.IO.File.Create(tempFileFullName);
        var fileStream = f.OpenReadStream();
        _ = fileStream.Seek(0, SeekOrigin.Begin);
        await fileStream.CopyToAsync(fs).ConfigureAwait(false);
        fs.Close();
        var extension = Path.GetExtension(tempFileFullName);
        if (!availableExtensions.Contains(extension))
        {
            // System.IO.File.Delete(tempFileFullName);
            return string.Empty;
        }

        return tempFileFullName;
    }
}
