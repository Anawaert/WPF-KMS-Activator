using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KMS_Activator
{
    class Program
    {

        #region 常量与静态变量区
        private const string ACTIVATE_WIN_CLI_INPUT = "WINDOWS";
        private const string ACTIVATE_OFFICE_CLI_INPUT = "OFFICE";
        private static string renewType = string.Empty;
        #endregion 常量与静态变量区

        [STAThread]
        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "renew")
                {
                    renewType = args[i + 1];
                    break;
                }
            }

            if (renewType.ToUpper().Trim(' ') == ACTIVATE_WIN_CLI_INPUT)
            {
                Win_Activator actWinFromCLI = new Win_Activator();
                actWinFromCLI.ActWin(Shared.AW_KMS_SERVER_ADDR);
                return;
            }
            else if (renewType.ToUpper().Trim(' ') == ACTIVATE_OFFICE_CLI_INPUT)
            {
                Office_Activator actOfficeFromCLI = new Office_Activator();
                actOfficeFromCLI.ActOffice(Shared.AW_KMS_SERVER_ADDR);
                return;
            }

            App activator = new App();
            activator.InitializeComponent();
            activator.Run();
        }
    }
}
