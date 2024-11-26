using System.Diagnostics;
using System.Net;
using Masuit.Tools;

public static class Command
{
    public static void Execute(string? cmd)
    {
        if (cmd == null)
        {
            return;
        }

        var processInfo = new ProcessStartInfo("powershell.exe", cmd)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            UserName = AppSetting.Config?.GetSection("UserName")?.Value,
            Password = new NetworkCredential("", AppSetting.Config?.GetSection("UserPassword")?.Value).SecurePassword,
        };

        var process = Process.Start(processInfo);
        process?.WaitForExit();
        process?.Close();
    }
}
