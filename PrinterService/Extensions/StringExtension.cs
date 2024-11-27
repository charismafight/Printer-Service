
using System.Configuration;

public static class StringExtension
{
    static string? printCMD;
    public static void InitPrintCommand(string? cmd)
    {
        printCMD = cmd;
    }

    // #if windows
    public static void Print(this string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(printCMD))
        {
            return;
        }

        var extension = Path.GetExtension(path);
        if (extension == ".pdf")
        {
            Command.PrintPDFs(path);
        }
        else
        {
            Command.Execute(printCMD.Replace("{Path}", path, StringComparison.CurrentCulture));
        }
    }

    // #endif
}
