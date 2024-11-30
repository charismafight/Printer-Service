using System.Diagnostics;
using System.Net;
using Masuit.Tools;

public static class Command
{
    public static void Execute(string? cmd, string args = "", bool waitExit = true)
    {
        if (cmd == null)
        {
            return;
        }

        var processInfo = new ProcessStartInfo(cmd, args);

        var process = Process.Start(processInfo);
        if (waitExit)
        {
            process?.WaitForExit();
            process?.Close();
        }
    }

    public static bool KillProcess(string? name)
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
