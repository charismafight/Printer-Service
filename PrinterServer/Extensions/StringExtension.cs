using System.Configuration;
using System.IO;

public static class StringExtension
{
    private static string? _adobe = ConfigurationManager.AppSettings["AdobePath"]?.ToString();

    // #if windows
    public static void Print(this string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        var extension = Path.GetExtension(path);
        if (extension == ".pdf")
        {
            Command.Execute(_adobe, $"/p /h \"{path}\"", false);
            Thread.Sleep(10000);
            Command.KillProcess("Acrobat");
        }
        else
        {
            Command.Execute("powershell.exe", $"Start-Process -FilePath '{path}' -Verb Print -Wait");
        }
    }

    // #endif
}
