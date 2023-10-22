using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using Wpf = System.Windows;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace KMS_Activator
{
    /// <summary>
    ///     <para>
    ///         该静态类主要用于实现程序内需要共享复用的代码，如进程执行、判断等
    ///     </para>
    ///     <para>
    ///         The static class is mainly used to realize the code that needs to be shared and reused in the program, such as process execution and judgment
    ///     </para>
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
        /// <param name="execname">
        ///     <para>
        ///         可执行程序名称
        ///     </para>
        ///     <para>
        ///         Name of an executable program
        ///     </para>
        /// </param>
        /// <param name="args">
        ///     <para>
        ///         需要传入的参数
        ///     </para>
        ///     <para>
        ///         Parameters that need to be passed in
        ///     </para>
        /// </param>
        /// <param name="workdir">
        ///     <para>
        ///         工作目录，若置空则认为是System32
        ///     </para>
        ///     <para>
        ///         The working directory, if empty, is considered System32
        ///     </para>
        /// </param>
        /// <param name="is_silent">
        ///     <para>
        ///         是否静默执行
        ///     </para>
        ///     <para>
        ///         Silent execution or not
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         一个 <see langword="string"/> 类型值，即被调用程序的输出值 
        ///     </para>
        ///     <para>
        ///         A <see langword="string"/> value, which is the output of the called program
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
            // 如果不需要
            // if it doesn't need
            else
            {
                // 将startInfo对象的“使用外壳程序”、“无新窗口创建”、“窗口类型”和“程序的标准输出流重抓取”属性进行设置
                // Set the "UseShellExecute", "CreateNoWindow", "WindowStyle" and "RedirectStandardOutput" properties of the startInfo object
                startInfo.UseShellExecute = true;
                startInfo.CreateNoWindow = false;
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.RedirectStandardOutput = true;
            }

            // 开始执行进程并使用try...catch块处理异常
            // Start the execution process and use try... The catch block handles exceptions
            Process process = new Process { StartInfo = startInfo };
            string output = string.Empty;
            try
            {
                process.Start();
                process.WaitForExit();
                output = process.StandardOutput.ReadToEnd();
            }
            catch
            {
                return "FATAL_ERROR";
            }

            // 获取输出并且等待进程的自动完成
            // Get the output and wait for the process to complete automatically
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
        ///         一个 <see langword="bool"/> 类型值，<see langword="true"/> 代表已激活，<see langword="false"/> 代表未激活  
        ///     </para>
        ///     <para>
        ///         A <see langword="bool"/> value where <see langword="true"/> means activated and <see langword="false"/> means not activated
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
        ///         一个 <see langword="bool"/>，<see langword="true"/> 则表示这是管理员账户
        ///     </para>
        ///     <para>
        ///         A <see langword="bool"/> and <see langword="true"/> means this is an administrator account
        ///     </para>
        /// </returns>
        public static bool IsAdministrators()
        {
            // 获取当前用户身份凭证并验证是否符合管理员规则
            // Get the current user's credentials and verify compliance with administrator rules
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        ///     <para>
        ///         该函数用于向schtasks.exe传递命令，以设定在180天后自动启动程序进行激活
        ///     </para>
        ///     <para>
        ///         This function is used to pass a command to schtasks.exe to set the program to start automatically for activation after 180 days
        ///     </para>
        /// </summary>
        /// <param name="renewTarget">
        ///     <para>
        ///         一个 <see langword="string"/> 类型值，需要传入续签的类型（Windows或Office）
        ///     </para>
        ///     <para>
        ///         A <see langword="string"/> value with the type to renew (Windows or Office)
        ///     </para>
        /// </param>
        public static void AutoRenewSign(string renewTarget)
        {
            Assembly? assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                // 将自身拷贝到“C:\Users\%username%\KMS Activator\”下后利用schtasks.exe设定定时任务，在180天后再次启动
                // Copy itself to "C:\Users\%username%\KMS Activator\" and use schtasks.exe to set a scheduled task and start it again after 180 days
                DirectoryInfo info = Directory.CreateDirectory(USER_DOC_PATH + "KMS Activator");
                File.Copy(assembly.Location, info.FullName + "\\Anawaert KMS Activator.exe", true);
                string exeName = "schtasks.exe";
                // "/sm"开关为"StartMode"的缩写，"renew"指示这次启动程序是以续签的身份启动，renew后面的参数"win"或"office"指示将续签什么类型的激活
                // The "/sm" switch is short for "StartMode", "renew" indicates that this time the boot program is started as a renewal, and the parameter "Windows" or "Office" after renew indicates what type of activation will be renewed
                string args = "/create /tn KMS_Renew_" + renewTarget + " /tr " + "\"" + info.FullName + "\\Anawaert KMS Activator.exe /sm renew " + renewTarget + "\" /sc daily /mo 180";
                RunProcess(exeName, args, string.Empty, true);
            }
        }

        /// <summary>
        ///     <para>
        ///         该函数用于向schtasks.exe传递命令，取消已经设定的启动任务
        ///     </para>
        ///     <para>
        ///         This function is used to pass a command to schtasks.exe to cancel a startup task that has already been set
        ///     </para>
        /// </summary>
        /// <param name="cancelRenewTarget">
        ///     <para>
        ///         一个 <see langword="string"/> 类型值，需要传入取消续签的类型（Windows或Office）
        ///     </para>
        ///     <para>
        ///         A <see langword="string"/> value that requires the type of cancellation (Windows or Office)
        ///     </para>
        /// </param>
        public static void CancelAutoRenew(string cancelRenewTarget)
        {
            string exeName = "schtasks.exe";
            string args = "schtasks /delete /tn KMS_Renew_" + cancelRenewTarget + " /f";
            RunProcess(exeName, args, string.Empty, true);
        }

        /// <summary>
        ///     <para>
        ///         该函数用于通过异步的方法检查Github上最新的Release版本号，以提示用户是否更新。该函数来自Anawaert USBHDDSpy
        ///     </para>
        ///     <para>
        ///         This function is used to asynchronously check the latest Release number on Github to prompt the user for an update. This function is from Anawaert USBHDDSpy
        ///     </para>
        /// </summary>
        public static async Task AutoCheckUpdate()
        {
            try
            {
                HttpClient NewClient = new HttpClient();
                HttpResponseMessage httpResponse = await NewClient.GetAsync("https://github.com/Anawaert/WPF-KMS-Activator");  // 连接至GitHub上USBHDDSpy的主页
                httpResponse.EnsureSuccessStatusCode();  // 确保Http正确相响应
                string ResponseBody = await httpResponse.Content.ReadAsStringAsync();  // 将相应返回的主页内容从字节码转为字符串

                Regex GetTitleRegex = new Regex(@"<title>.+?</title>");  // 匹配<title>与</title>标签之间的全部内容
                Regex GetDateFromTitleRegex = new Regex("");  // 匹配<title>与</title>标签之间以xxxx-xx为格式的日期字符串。此处为什么要使用两次正则呢，因为经实测如果仅使用本行代码的正则规则匹配，可能导致匹配到非希望的结果。
                if (GetDateFromTitleRegex.Match(GetTitleRegex.Match(ResponseBody).Value).Value != "2023-05")
                {
                    DialogResult result = MessageBox.Show("当前更新可用，是否现在进行更新并导航至下载界面？", "USBHDDSpy Update", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);  // 弹出对话框
                    if (result == DialogResult.Yes)  // 单击“是”
                    {
                        try
                        {
                            Process ShellCmd = new Process();
                            ShellCmd.StartInfo.FileName = "cmd.exe";
                            ShellCmd.StartInfo.RedirectStandardInput = true; ShellCmd.StartInfo.UseShellExecute = false; ShellCmd.StartInfo.CreateNoWindow = true;  // 隐式调用cmd.exe
                            ShellCmd.Start();
                            ShellCmd.StandardInput.WriteLine("EXPLORER \"https://github.com/Anawaert-Download/USBHDDSpy_Download/archive/refs/heads/main.zip\" & EXIT");  // 使用cmd语句访问下载链接。由于Anawaert无条件建立一个24小时开放的直链下载服务，突发奇想把Github作为下载源，经实践暂且认为可行。
                            ShellCmd.Dispose();  // 释放内存
                        }
                        catch { MessageBox.Show("连接至GitHub时发生错误，更新已取消", "USBHDDSpy 消息", MessageBoxButtons.OK); }  // 当网络连接不通畅或者无网络连接时
                    }
                    else if (result == DialogResult.Cancel)  // 单击“取消”时
                    {
                        if (MessageBox.Show("是否取消更新？\n\n若单击“是”，则将屏蔽自动更新，您需要手动访问GitHub以获取更新；若单击“否”，则将取消本次更新，但不会屏蔽", "USBHHDDSpy 消息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            // MainConsole.UpdateOrNot = "0";  // 将MainConsole中UpdateOrNot静态变量改为"0"，这样以后就不检查更新

                        }
                    }
                }
            }
            catch { }
        }
        
        //public static void ChangeLabelFontFamily(string targetFontFamily, Wpf::Controls.Label targetLabel)
        //{
        //    mainWindow.Dispatcher.Invoke
        //    (
        //        () =>
        //        {
        //            targetLabel.FontFamily = new Wpf::Media.FontFamily(targetFontFamily);
        //        }
        //    );
        //}

        //public static void ChangeGroupBoxVisibility(Wpf::Visibility visibility, Wpf::Controls.GroupBox targetGroupBox)
        //{
        //    mainWindow.Dispatcher.Invoke
        //    (
        //        () =>
        //        {
        //            targetGroupBox.Visibility = visibility;
        //        }
        //    );
        //}

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
        /// <summary>
        ///     <para>
        ///         该值指示当前用户的“文档”文件夹
        ///     </para>
        ///     <para>
        ///         This value indicates the current user's Documents folder
        ///     </para>
        /// </summary>
        public static string USER_DOC_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
        /// <summary>
        ///     <para>
        ///         该变量为运行在UI线程上的Window类实例
        ///     </para>
        ///     <para>
        ///         This variable is an instance of the Window class that runs on the UI thread
        ///     </para>
        /// </summary>
        public static MainWindow mainWindow = (MainWindow)Wpf::Application.Current.MainWindow;
        #endregion
    }
}