using Serilog;
using System;
using System.Diagnostics;

namespace Activator
{
    public partial class Utility
    {
        /// <summary>
        /// <para>以指定的参数运行一个进程</para>
        /// <para>Run a process with specified parameters</para>
        /// </summary>
        /// <param name="execName">
        /// <para>要执行的进程的名称，如 "cmd.exe"</para>
        /// <para>Name of the process to be executed, such as "cmd.exe"</para>
        /// </param>
        /// <param name="execParams">
        /// <para>要传递给进程的参数，如 "/c echo Hello World"，可为空</para>
        /// <para>Parameters to be passed to the process, such as "/c echo Hello World", can be null</para>
        /// </param>
        /// <param name="workingDirectory">
        /// <para>进程的工作目录，如 "D:\"，若为空则默认为 "C:\Windows\System32"</para>
        /// <para>Working directory of the process, such as "D:\", if null then default to "C:\Windows\System32"</para>
        /// </param>
        /// <param name="isSilent">
        /// <para>是否以静默模式运行进程，即不显示进程的窗口</para>
        /// <para>Whether to run the process in silent mode, that is, without displaying the window of the process</para>
        /// </param>
        /// <returns>
        /// <para>进程的标准输出</para>
        /// <para>Standard output of the process</para>
        /// </returns>
        public static string RunProcess(string execName, string? execParams, string? workingDirectory, bool isSilent)
        {
            // 声明一个在特定目录下的进程启动对象（默认假设在 C:\Windows\System32\ ）
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

            try
            {
                Process process = new Process() { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();

                Log.Logger.Information("Ran process: {0} with parameters: {1} in directory: {2}", execName, execParams, workingDirectory ?? Shared.System32Path);

                return process.StandardOutput.ReadToEnd(); 
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to run process: {0} with parameters: {1} in directory: {2}", execName, execParams, workingDirectory ?? Shared.System32Path);
                return string.Empty;
            }
        }
    }
}
