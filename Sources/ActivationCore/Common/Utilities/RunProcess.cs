using System;
using System.Diagnostics;

namespace Activator
{
    public partial class Utility
    {
        public static string RunProcess(string execName, string? execParams, string? workingDirectory, bool isSilent)
        {
            // 声明一个在特定目录下的进程启动对象（默认假设在C:\Windows\System32\）
            // Declare a process start object under a specific directory (supposed that it is C:\Windows\System32\) 
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = execName,
                WorkingDirectory = workingDirectory ?? Shared.System32Path,
                Arguments = execParams ?? string.Empty,
                Verb = "RunAs",
                UseShellExecute = !isSilent,
                CreateNoWindow = isSilent,
                WindowStyle = isSilent ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                RedirectStandardOutput = true
            };

            // 开始执行进程
            // Start the process
            try
            {
                Process process = new Process() { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd(); 
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
