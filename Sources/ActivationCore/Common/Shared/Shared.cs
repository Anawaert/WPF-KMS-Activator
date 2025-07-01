using Serilog;
using System;

namespace Activator
{
    /// <summary>
    /// <para>本类主要包含一些共享信息</para>
    /// <para>This class mainly contains some shared information</para>
    /// </summary>
    public class Shared
    {
        /// <summary>
        /// <para>Windows 中 cscript.exe 的路径</para>
        /// <para>Path of cscript.exe in Windows</para>
        /// </summary>
        public static string Cscript { get; private set; } = "cscript.exe";

        /// <summary>
        /// <para>Windows 中 dism.exe 的路径</para>
        /// <para>Path of dism.exe in Windows</para>
        /// </summary>
        public static string Dism { get; private set; } = "dism.exe";

        /// <summary>
        /// <para>Anawaert 服务器地址</para>
        /// <para>Anawaert server address</para>
        /// </summary>
        public static string AnawaertServerAddress { get; private set; } = "www.anawaert.tech";

        /// <summary>
        /// <para>应用程序当前运行目录</para>
        /// <para>Current running directory of the application</para>
        /// </summary>
        public static string? ApplicationCurrentDirectory { get; private set; }

        /// <summary>
        /// <para>Windows 系统 System32 目录</para>
        /// <para>System32 directory of Windows system</para>
        /// </summary>
        public static string? System32Path { get; private set; }

        /// <summary>
        /// <para>用户文档目录</para>
        /// <para>User documents directory</para>
        /// </summary>
        public static string? UserDocumentsPath { get; private set; }

        /// <summary>
        /// <para>用户文档目录下 Anawaert KMS Activator 目录</para>
        /// <para>Anawaert KMS Activator directory under user documents directory</para>
        /// </summary>
        public static string? UserDocumentsActivatorPath { get; private set; }

        /// <summary>
        /// <para>JSON 配置文件路径</para>
        /// <para>Path of JSON configuration file</para>
        /// </summary>
        public static string? JsonFilePath { get; private set; }

        /// <summary>
        /// <para>初始化共享信息</para>
        /// <para>Initialize shared information</para>
        /// </summary>
        public static void InitializeSharedInfo()
        {
            ApplicationCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\";
            System32Path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\";
            UserDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
            UserDocumentsActivatorPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Anawaert KMS Activator\\";
            JsonFilePath = UserDocumentsActivatorPath + "Activator Configuration.json";
            
            Log.Logger.Information("Initialized or refreshed shared information.");
        }
    }
}
