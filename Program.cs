using System;
using Serilog;
using Activator.Models;

namespace Activator
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            // 初始化日志对象
            // Initialize the log object
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Shared.UserDocumentsActivatorPath is not null ?
                              Shared.UserDocumentsActivatorPath + "Activator Log.log" :
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Anawaert KMS Activator\\Activator Log.log",
                              outputTemplate: @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {NewLine} {Exception}")
                .CreateLogger();

            // 使用同步的方法获取配置文件以确保配置文件加载完成
            // Use synchronous methods to get the configuration file to ensure that the configuration file is loaded
            AllSettingsModel.Instance = ModelOperation.GetConfig<AllSettingsModel>
            (
                Shared.JsonFilePath ??
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                "\\Anawaert KMS Activator\\Activator Configuration.json"
            );

            App activator = new App();
            activator.InitializeComponent();
            activator.Run();

            return 0x00;
        }
    }
}
