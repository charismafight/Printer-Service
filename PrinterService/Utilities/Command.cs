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
        };

        var process = Process.Start(processInfo);
        process?.WaitForExit();
        process?.Close();
    }

    public static Boolean PrintPDFs(string pdfFileName)
    {
        try
        {
            Process proc = new Process();
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.Verb = "print";

            //Define location of adobe reader/command line
            //switches to launch adobe in "print" mode
            proc.StartInfo.FileName = AppSetting.Config?.GetSection("AdobePath").Value;
            proc.StartInfo.Arguments = string.Format($@"/p /h {{0}}", pdfFileName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (proc.HasExited == false)
            {
                proc.WaitForExit(10000);
            }

            proc.EnableRaisingEvents = true;

            proc.Close();
            KillAdobe(AppSetting.Config?.GetSection("AdobeTaskName").Value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool KillAdobe(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        foreach (Process clsProcess in Process.GetProcesses().Where(
                     clsProcess => clsProcess.ProcessName.StartsWith(name)))
        {
            clsProcess.Kill();
        }
        return true;
    }
}
