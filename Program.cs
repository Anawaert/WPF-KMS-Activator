using System;
using System.IO;
using System.Text;
using System.Windows;
using static KMS_Activator.Shared;

namespace KMS_Activator
{
    class Program
    {

        #region 常量与静态变量区
        private const string ACTIVATE_WIN_CLI_INPUT = "WINDOWS";
        private const string ACTIVATE_OFFICE_CLI_INPUT = "OFFICE";
        private const short ERROR_PROCESS = -1;
        private const short FULLY_FINISHED = 0x00;
        private const short RENEW_WIN_DONE = 0x01;
        private const short RENEW_OFFICE_DONE = 0x02;
        #endregion 常量与静态变量区

        [STAThread]
        public static int Main(string[] args)
        {
            string renewType = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--renew" && args[i + 1] != null)
                {
                    renewType = args[i + 1];
                    break;
                }
            }

            // 新增：先判断文档文件夹下的KMS Activator文件夹是否存在，然后加载JSON格式的配置文件
            if (!Directory.Exists(USER_DOC_KMS_PATH.TrimEnd('\\')))
            {
                DirectoryInfo createInfo = CreateDirInUserDocuments();
                USER_DOC_KMS_PATH = createInfo.FullName + "\\";
            }
            if (!File.Exists(JSON_CFG_PATH))
            {
                RefreshConfigInit();
                RefreshConfigFile(JSON_CFG_PATH);
            }
            ReadConfigFromFile(JSON_CFG_PATH);

            if (renewType.ToUpper().Trim(' ') == ACTIVATE_WIN_CLI_INPUT)
            {
                MessageBoxResult result = MessageBox.Show
                (
                    "您的Windows系统激活时间或马上到期，即将开始对Windows系统KMS激活的续签，点击“确认”以继续，点击“取消”以取消续签",
                    "Anawaert KMS Activator 提示",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information
                );
                if (result == MessageBoxResult.OK)
                {
                    Win_Activator actWinFromCLI = new Win_Activator();
                    actWinFromCLI.ActWin(AW_KMS_SERVER_ADDR);
                }

                if (Current_Config.isAutoRenew)
                {
                    CancelAutoRenew("WINDOWS");
                    AutoRenewSign("WINDOWS");
                }
                else
                {
                    CancelAutoRenew("WINDOWS");
                }
                return RENEW_WIN_DONE;
            }
            else if (renewType.ToUpper().Trim(' ') == ACTIVATE_OFFICE_CLI_INPUT)
            {
                MessageBoxResult result = MessageBox.Show
                (
                    "您的Office软件激活时间或马上到期，即将开始对Office软件KMS激活的续签，点击“确认”以继续，点击“取消”以取消续签",
                    "Anawaert KMS Activator 提示",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information
                );
                if (result == MessageBoxResult.OK)
                {
                    Office_Activator actOfficeFromCLI = new Office_Activator();
                    actOfficeFromCLI.ActOffice(AW_KMS_SERVER_ADDR);
                }

                if (Current_Config.isAutoRenew)
                {
                    CancelAutoRenew("OFFICE");
                    AutoRenewSign("OFFICE");
                }
                else
                {
                    CancelAutoRenew("OFFICE");
                }
                return RENEW_OFFICE_DONE;
            }

            App activator = new App();
            activator.InitializeComponent();
            activator.Run();

            return FULLY_FINISHED;
        }
    }
}
