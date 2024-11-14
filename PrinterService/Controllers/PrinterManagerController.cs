using System.Drawing;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
internal class PrinterManagerController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(IFormFile? file)
    {
        if (file is null)
        {
            return BadRequest();
        }

        var tempFileName = Guid.NewGuid().ToString();
        using var fs = System.IO.File.Create(tempFileName);
        var fileStream = file.OpenReadStream();
        _ = fileStream.Seek(0, SeekOrigin.Begin);
        await fileStream.CopyToAsync(fs);

        // execute printer command to print document
        tempFileName.PrintImage();

        return NoContent();
    }
}
