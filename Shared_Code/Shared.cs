using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using System.Windows;

namespace KMS_Activator
{
    /// <summary>
    /// <para>
    /// 该静态类主要用于实现程序内需要共享复用的代码，如进程执行、判断等
    /// </para>
    /// <para>
    /// The static class is mainly used to realize the code that needs to be shared and reused in the program, such as process execution and judgment
    /// </para>
    /// </summary>
    public static class Shared
    {
        /// <summary>
        ///     <para>
        ///         该静态函数用于启动位于特定目录下的一些重要系统内置程序以实现某些功能
        ///     </para>
        ///     <para>
        ///         This static function is used to launch some important system built-in programs located in the specific directory to implement certain functions
        ///     </para>
        /// </summary>
        /// <param name="execname">可执行程序名称 <br/> Name of an executable program</param>
        /// <param name="args">需要传入的参数 <br/> Parameters that need to be passed in</param>
        /// <param name="workdir">工作目录，若置空则认为是System32 <br/> The working directory, if empty, is considered System32</param>
        /// <param name="is_silent">是否静默执行 <br/> Silent execution or not</param>
        /// <returns>
        ///     <para>
        ///         被调用程序的输出值 
        ///     </para>
        ///     <para>
        ///         The output value of the called program
        ///     </para>
        /// </returns>
        public static string RunProcess(string execname, string args, string workdir, bool is_silent)
        {
            // 声明一个在特定目录下的进程启动对象（默认假设在C:\Windows\System32\）
            // Declare a process start object under a specific directory (supposed that it is C:\Windows\System32\) 
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = execname,
                WorkingDirectory = workdir == string.Empty ? SYS32_PATH : workdir,
                Arguments = args,
            };  
            // 如果需要静默运行
            // If it needs to run silently
            if (is_silent) 
            {               
                // 将startInfo对象的“使用外壳程序”、“无新窗口创建”、“窗口类型”和“程序的标准输出流重抓取”属性进行设置
                // Set the "UseShellExecute", "CreateNoWindow", "WindowStyle" and "RedirectStandardOutput" properties of the startInfo object
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
            }

            // 开始执行进程并使用try...catch块处理异常
            // Start the execution process and use try... The catch block handles exceptions
            Process process = new Process{ StartInfo = startInfo };
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                /*****待补充*****/
                return "FATAL_ERROR";
            }

            // 获取输出并且等待进程的自动完成
            // Get the output and wait for the process to complete automatically
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }       

        /// <summary>
        ///     <para>
        ///         该函数用于Windows判断是否已经激活
        ///     </para> 
        ///     <para>
        ///         This function is used to determine whether it has been activated
        ///     </para>
        /// </summary>
        /// <returns>
        ///     <para>
        ///         一个<see langword="bool"/>，true代表已激活，false代表未激活  
        ///     </para>
        ///     <para>
        ///         A Boolean value that true indicates activated and false indicates inactivated
        ///     </para>
        /// </returns>
        public static bool IsWinActivated()
        {
            // 通过cscript.exe执行slmgr.vbs后获取输出
            // The output is obtained after slmgr.vbs is executed using cscript.exe
            string slmgr_output = RunProcess(CSCRIPT, @"//NoLogo slmgr.vbs /dli", string.Empty, true);
            // 若输出结果中包含以下关键词，则说明已经激活
            // If the output contains the following keywords, it is activated
            return slmgr_output.Contains("license status: licensed") || slmgr_output.Contains("已授权");
        }
    
        /// <summary>
        ///     <para>
        ///         该函数用于验证当前登陆的账户是否为管理员账户
        ///     </para>
        ///     <para>
        ///         This function is used to verify that the current login account is an administrator account
        ///     </para>
        /// </summary>
        /// <returns>
        ///     <para>
        ///         一个<see langword="bool"/>，true则表示这是管理员账户
        ///     </para>
        ///     <para>
        ///         A Boolean value, and true indicates that this is an administrator account
        ///     </para>
        /// </returns>
        public static bool IsAdministrators()
        {
            // 获取当前用户身份凭证并验证是否符合管理员规则
            // Get the current user's credentials and verify compliance with administrator rules
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }   

        public static void AutoRenewSign(bool isAutoRenew)
        {
            if (isAutoRenew)
            {
                DirectoryInfo info = Directory.CreateDirectory(USER_DOC_PATH + "KMS Activator");
                File.Copy(EXEC_PATH + "Renew.exe", info.FullName + "\\Renew.exe");
                string exeName = @"schtasks.exe";
                string args = "/create /tn \"KMS_Renew\" /tr " + info.FullName + "\\Renew.exe" + " /sc ONLOGON /mo 180";
                RunProcess(exeName, args, string.Empty, false);
            }
            return;
        }

        #region 全局静态变量与常量区
        /// <summary>
        ///     <para>
        ///         该常量的值为cscript.exe
        ///     </para>
        ///     <para>
        ///         This value is an alias for cscript.exe
        ///     </para>
        /// </summary>
        public const string CSCRIPT = "cscript.exe";
        /// <summary>
        ///     <para>
        ///         该静态变量指示当前应用程序运行时所在的目录
        ///     </para>
        ///     <para>
        ///         This static variable indicates the directory where the current application is running
        ///     </para>
        /// </summary>
        public static string EXEC_PATH = AppDomain.CurrentDomain.BaseDirectory + "\\";
        /// <summary>
        ///     <para>
        ///         该静态量用以获取存储当前Windows系统的产品名或版本名
        ///     </para>
        ///     <para>
        ///         This static quantity is used to obtain the product name or version name that stores the current Windows system
        ///     </para>
        /// </summary>
        public static string WIN_VERSION = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", string.Empty)?.ToString() ?? "Not_Found";
        /// <summary>
        ///     <para>
        ///         该值指示当前系统的System32目录的绝对路径
        ///     </para>
        ///     <para>
        ///         This value indicates the absolute path to the System32 directory on the current system
        ///     </para>
        /// </summary>
        public static string SYS32_PATH = Environment.GetEnvironmentVariable("SystemRoot") + "\\System32";

        public static string USER_DOC_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";

        public static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        #endregion
    }
}