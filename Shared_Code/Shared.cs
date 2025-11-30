using System;
using System.Security.Principal;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;


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
        ///         该常量为Anawaert的KMS服务器地址
        ///     </para>
        ///     <para>
        ///         This value is the address of Anawaert KMS server
        ///     </para>
        /// </summary>
        public const string AW_KMS_SERVER_ADDR = "anawaert.com";
        /// <summary>
        ///     <para>
        ///         该静态变量指示当前应用程序运行时所在的目录
        ///     </para>
        ///     <para>
        ///         This static variable indicates the directory where the current application is running
        ///     </para>
        /// </summary>
        public static string EXEC_PATH { get; } = AppDomain.CurrentDomain.BaseDirectory + "\\";
        /// <summary>
        ///     <para>
        ///         该静态量用以获取存储当前Windows系统的产品名或版本名
        ///     </para>
        ///     <para>
        ///         This static quantity is used to obtain the product name or version name that stores the current Windows system
        ///     </para>
        /// </summary>
        public static string WIN_VERSION { get; } = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", string.Empty)?.ToString() ?? "Not_Found";
        /// <summary>
        ///     <para>
        ///         该值指示当前系统的System32目录的绝对路径
        ///     </para>
        ///     <para>
        ///         This value indicates the absolute path to the System32 directory on the current system
        ///     </para>
        /// </summary>
        public static string SYS32_PATH { get; } = Environment.GetEnvironmentVariable("SystemRoot") + "\\System32";
        /// <summary>
        ///     <para>
        ///         该值指示当前用户的“文档”文件夹
        ///     </para>
        ///     <para>
        ///         This value indicates the current user's Documents folder
        ///     </para>
        /// </summary>
        public static string USER_DOC_PATH { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
        /// <summary>
        ///     <para>
        ///         该变量为运行在UI线程上的Window类实例
        ///     </para>
        ///     <para>
        ///         This variable is an instance of the Window class that runs on the UI thread
        ///     </para>
        /// </summary>
        internal static MainWindow? mainWindow { get; set; }
        /// <summary>
        ///     <para>
        ///         该变量为KMS Activator在用户的文档目录下的工作目录(C:\Users\%USERNAME%\Documents\KMS Activator\)
        ///     </para>
        ///     <para>
        ///         This variable is the KMS Activator working directory in the user's Documents directory (C:\Users\%USERNAME%\Documents\KMS Activator\)
        ///     </para>
        /// </summary>        
        public static string USER_DOC_KMS_PATH { get; set; } = USER_DOC_PATH + "KMS Activator\\";
        /// <summary>
        ///     <para>
        ///         该变量为KMS Activator在用户的文档目录下的工作目录中配置文件的路径
        ///     </para>
        ///     <para>
        ///         This variable is the path to the KMS Activator profile in the user's working directory under the document directory
        ///     </para>
        /// </summary>  
        public static string JSON_CFG_PATH { get; } = USER_DOC_KMS_PATH + "cfg.json";

        #endregion 全局静态变量与常量区

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
        ///         一个 <see cref="string"/> 类型值，即被调用程序的输出值 
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value, which is the output of the called program
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
                Verb = "RunAs"
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
        ///         一个 <see cref="bool"/> 类型值，<see langword="true"/> 代表已激活，<see langword="false"/> 代表未激活  
        ///     </para>
        ///     <para>
        ///         A <see cref="bool"/> value where <see langword="true"/> means activated and <see langword="false"/> means not activated
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
        ///         一个 <see cref="bool"/>，<see langword="true"/> 则表示这是管理员账户
        ///     </para>
        ///     <para>
        ///         A <see cref="bool"/> and <see langword="true"/> means this is an administrator account
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
        ///         一个 <see cref="string"/> 类型值，需要传入续签的类型（"Windows"或"Office"）
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value with the type to renew ("Windows" or "Office")
        ///     </para>
        /// </param>
        public static void AutoRenewSign(string renewTarget)
        {
            string exec_in_user_doc_path = USER_DOC_KMS_PATH + "Activator.exe";
            ProcessModule? module = Process.GetCurrentProcess().MainModule;
            string? currentPath = module!.FileName;
            if (currentPath != null)
            {
                // 将自身拷贝到“C:\Users\%username%\KMS Activator\”下后利用schtasks.exe设定定时任务，在180天内再次启动
                // Copy itself to "C:\Users\%username%\KMS Activator\" and use schtasks.exe to set a scheduled task and start it again in 180 days
                if (currentPath != exec_in_user_doc_path)
                {
                    File.Copy(currentPath, exec_in_user_doc_path, true);
                }
                string exeName = "schtasks.exe";
                // "/sm"开关为"StartMode"的缩写，"renew"指示这次启动程序是以续签的身份启动，--renew后面的参数"windows"或"office"指示将续签什么类型的激活，/rl指示使用最高权限启动
                // The "/sm" switch is short for "StartMode", "renew" indicates that this time the boot program is started as a renewal, and the parameter "Windows" or "Office" after renew indicates what type of activation will be renewed
                string args = "/CREATE /TN \\Anawaert\\KMS_Renew_" + renewTarget + " /TR " + "\"\\\"" + USER_DOC_KMS_PATH + "Activator.exe\\\" --renew " + renewTarget + "\" /SC DAILY /MO 180 /RL HIGHEST";
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
        ///         一个 <see cref="string"/> 类型值，需要传入取消续签的类型（"Windows"或"Office"）
        ///     </para>
        ///     <para>
        ///         A <see cref="string"/> value that requires the type of cancellation ("Windows" or "Office")
        ///     </para>
        /// </param>
        public static void CancelAutoRenew(string cancelRenewTarget)
        {
            string exeName = "schtasks.exe";
            string args = "/delete /tn \\Anawaert\\KMS_Renew_" + cancelRenewTarget + " /f";
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
                HttpClient newClient = new HttpClient();
                HttpResponseMessage httpResponse = await newClient.GetAsync("https://github.com/Anawaert/WPF-KMS-Activator/releases");  // 连接至GitHub上的主页
                httpResponse.EnsureSuccessStatusCode();  // 确保Http正确相响应
                string responseBody = await httpResponse.Content.ReadAsStringAsync();  // 将相应返回的主页内容从字节码转为字符串

                Regex getSpanTagsRegex = new Regex(@"<span(.*?)</span>");  // 匹配<span>与</span>标签之间的全部内容,但考虑到<span>标签可能包含其他的样式，故仅匹配<span到</span>之间
                Regex get_a_tags_regex = new Regex(@"<a(.*?)</a>");  // 匹配<a>与</a>标签之间的内容
                Regex getVersionNumsRegex = new Regex(@"(\d+\.\d+\.\d+\.\d+)");  // 匹配<span>与</span>标签之间以x.x.x.x为格式的版本号

                // 先将HTML字符串中所有<span></span>标签之间的内容捕获，然后合并成一个新的大字符串
                MatchCollection spanTagsRegexCollections = getSpanTagsRegex.Matches(responseBody);
                StringBuilder spanTagsStringBuilder = new StringBuilder();
                foreach (Match spanTag in spanTagsRegexCollections.Cast<Match>())
                {
                    spanTagsStringBuilder.Append(spanTag.Value);
                }

                // 然后将<span></span>组成的大字符串中包含<a></a>的内容捕获，Release页每个发行版的标题就在<a></a>里面
                MatchCollection a_tags_regex_collections = get_a_tags_regex.Matches(spanTagsStringBuilder.ToString());
                StringBuilder a_tags_string_builder = new StringBuilder();
                foreach (Match a_tag in a_tags_regex_collections.Cast<Match>())
                {
                    a_tags_string_builder.Append(a_tag.Value);
                }

                // 再从<a></a>组成的“小”字符串中匹配版本号，第一个匹配就是最新版本的版本号
                Match versionNumsMatch = getVersionNumsRegex.Match(a_tags_string_builder.ToString());
                if (versionNumsMatch.Value != "2.0.0.1" && versionNumsMatch.Success)
                {
                    MessageBoxResult result = MessageBox.Show
                    (
                        "当前更新可用，是否现在跳转至下载界面以获取更新？", 
                        "软件更新", 
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information,
                        MessageBoxResult.No
                    );
                    if (result == MessageBoxResult.Yes)  // 单击“是”
                    {
                        try
                        {
                            Process ShellCmd = new Process();
                            ShellCmd.StartInfo.FileName = "cmd.exe";
                            ShellCmd.StartInfo.RedirectStandardInput = true; ShellCmd.StartInfo.UseShellExecute = false; ShellCmd.StartInfo.CreateNoWindow = true;  // 隐式调用cmd.exe
                            ShellCmd.Start();
                            ShellCmd.StandardInput.WriteLine("EXPLORER \"https://github.com/Anawaert/WPF-KMS-Activator/releases\" & EXIT");  // 使用cmd语句访问下载链接。
                            ShellCmd.Dispose();  // 释放内存
                        }
                        catch { MessageBox.Show("连接至GitHub时发生错误，导航已取消", "消息提示", MessageBoxButton.OK, MessageBoxImage.Information); }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        ///     <para>
        ///         该函数用于刷新程序中指示当前程序内设置的全局静态变量
        ///     </para>
        ///     <para>
        ///         This function is used to refresh the global static variables in the program indicating the current setting within the program
        ///     </para>
        /// </summary>
        public static void RefreshConfigInit()
        {
            Current_Config.isAutoRenew = mainWindow?.autoRenew_CheckBox.IsChecked ?? true;
            Current_Config.isAutoUpdate = mainWindow?.autoUpdate_CheckBox.IsChecked ?? true;
        }

        /// <summary>
        ///     <para>
        ///         该函数用于将程序后台中的全局静态变量刷新写入至配置文件中
        ///     </para>
        ///     <para>
        ///         This function is used to write the global static variable flush in the background of the program to the configuration file
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     <para>
        ///         配置文件的绝对路径，使用一个<see cref="string"/>类型变量表示
        ///     </para>   
        ///     <para>
        ///         The absolute path to the configuration file, represented by a <see cref="string"/> variable
        ///     </para>        
        /// </param>
        public static void RefreshConfigFile(string path)
        {
            Config_Type config = new Config_Type()
            {
                IsAutoUpdate = Current_Config.isAutoUpdate,
                IsAutoRenew = Current_Config.isAutoRenew,
                IsDarkMode = Current_Config.isDarkMode
            };
            ConfigOperations.WriteConfig(config, path);
        }

        /// <summary>
        ///     <para>
        ///         该函数用于将配置文件的内容读取到程序静态变量中
        ///     </para>
        ///     <para>
        ///         This function is used to read the contents of the configuration file and store them into static variables
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     <para>
        ///         配置文件的绝对路径，使用一个<see cref="string"/>类型变量表示
        ///     </para>   
        ///     <para>
        ///         The absolute path to the configuration file, represented by a <see cref="string"/> variable
        ///     </para>        
        /// </param>
        public static void ReadConfigFromFile(string path)
        {
            Config_Type config = ConfigOperations.ReadConfig<Config_Type>(path);

            Current_Config.isAutoUpdate = config.IsAutoUpdate;
            Current_Config.isAutoRenew = config.IsAutoRenew;
            Current_Config.isDarkMode = config.IsDarkMode;

        }

        /// <summary>
        ///     <para>
        ///         该函数用于将（用于配置的）静态变量中的值应用到UI界面
        ///     </para>
        ///     <para>
        ///         This function is used to apply values stored in static variables that are used to be configurations to UI
        ///     </para>
        /// </summary>
        public static void EnableConfigToUI()
        {
            Action actionUI = () =>
            {
                mainWindow!.autoRenew_CheckBox.IsChecked = Current_Config.isAutoRenew;
                mainWindow!.autoUpdate_CheckBox.IsChecked = Current_Config.isAutoUpdate;
            };
            mainWindow!.Dispatcher.Invoke(actionUI);
        }

        /// <summary>
        ///     <para>
        ///         该函数用于为KMS Activator在用户的文档目录下创建程序需要的目录(C:\Users\%USERNAME%\Documents\KMS Activator\)
        ///     </para>
        ///     <para>
        ///         This function is used to create a directory in the user's Documents directory for the KMS Activator for the application (C:\Users\%USERNAME%\Documents\KMS Activator\)
        ///     </para>
        /// </summary>
        public static DirectoryInfo CreateDirInUserDocuments()
        {
            DirectoryInfo info = Directory.CreateDirectory(USER_DOC_PATH + "KMS Activator");
            return info;
        }
    }
}