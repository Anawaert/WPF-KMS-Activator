using System;
using System.Collections.Generic;

namespace Activator
{
    public class WindowsActivationKeys
    {
        public static Dictionary<WindowsProductName, string> OnlineActivationKeys { get; } = new Dictionary<WindowsProductName, string>()
        {
            { WindowsProductName.Windows_11_Pro, "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
            { WindowsProductName.Windows_11_Pro_N, "MH37W-N47XK-V7XM9-C7227-GCQG9" },
            { WindowsProductName.Windows_11_Pro_Education, "6TP4R-GNPTD-KYYHQ-7B7DP-J447Y" },
            { WindowsProductName.Windows_11_Pro_Education_N, "YVWGF-BXNMC-HTQYQ-CPQ99-66QFC" },
            { WindowsProductName.Windows_11_Education, "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2" },
            { WindowsProductName.Windows_11_Education_N, "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ" },
            { WindowsProductName.Windows_11_Enterprise, "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
            { WindowsProductName.Windows_11_Enterprise_N, "DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4" },
            { WindowsProductName.Windows_11_Enterprise_G, "YYVX9-NTFWV-6MDM3-9PT4T-4M68B" },
            { WindowsProductName.Windows_11_Enterprise_G_N, "44RPN-FTY23-9VTTB-MP9BX-T84FV" },
              
            { WindowsProductName.Windows_10_Pro, "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
            { WindowsProductName.Windows_10_Pro_N, "MH37W-N47XK-V7XM9-C7227-GCQG9" },
            { WindowsProductName.Windows_10_Pro_Education, "6TP4R-GNPTD-KYYHQ-7B7DP-J447Y" },
            { WindowsProductName.Windows_10_Pro_Education_N, "YVWGF-BXNMC-HTQYQ-CPQ99-66QFC" },
            { WindowsProductName.Windows_10_Education, "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2" },
            { WindowsProductName.Windows_10_Education_N, "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ" },
            { WindowsProductName.Windows_10_Enterprise, "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
            { WindowsProductName.Windows_10_Enterprise_N, "DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4" },
            { WindowsProductName.Windows_10_Enterprise_G, "YYVX9-NTFWV-6MDM3-9PT4T-4M68B" },
            { WindowsProductName.Windows_10_Enterprise_G_N, "44RPN-FTY23-9VTTB-MP9BX-T84FV" },
            { WindowsProductName.Windows_10_Enterprise_LTSC_2021, "M7XTQ-FN8P6-TTKYV-9D4CC-J462D" },
            { WindowsProductName.Windows_10_Enterprise_LTSC_2019, "M7XTQ-FN8P6-TTKYV-9D4CC-J462D" },
            { WindowsProductName.Windows_10_Enterprise_N_LTSC_2021, "92NFX-8DJQP-P6BBQ-THF9C-7CG2H" },
            { WindowsProductName.Windows_10_Enterprise_N_LTSC_2019, "92NFX-8DJQP-P6BBQ-THF9C-7CG2H" },
            { WindowsProductName.Windows_10_Enterprise_LTSB_2016, "DCPHK-NFMTC-H88MJ-PFHPY-QJ4BJ" },
            { WindowsProductName.Windows_10_Enterprise_N_LTSB_2016, "QFFDN-GRT3P-VKWWX-X7T3R-8B639" },
            { WindowsProductName.Windows_10_Enterprise_LTSB_2015, "WNMTR-4C88C-JK8YV-HQ7T2-76DF9" },
              
            { WindowsProductName.Windows_8_Point_1_Pro, "GCRJD-8NW9H-F2CDX-CCM8D-9D6T9" },
            { WindowsProductName.Windows_8_Point_1_Pro_N, "HMCNV-VVBFX-7HMBH-CTY9B-B4FXY" },
            { WindowsProductName.Windows_8_Point_1_Enterprise, "MHF9N-XY6XB-WVXMC-BTDCT-MKKG7" },
            { WindowsProductName.Windows_8_Point_1_Enterprise_N, "TT4HM-HN7YT-62K67-RGRQJ-JFFXW" },
              
            { WindowsProductName.Windows_8_Pro, "NG4HW-VH26C-733KW-K6F98-J8CK4" },
            { WindowsProductName.Windows_8_Pro_N, "XCVCF-2NXM9-723PB-MHCB7-2RYQQ" },
            { WindowsProductName.Windows_8_Enterprise, "32JNW-9KQ84-P47T8-D8GGY-CWCK7" },
            { WindowsProductName.Windows_8_Enterprise_N, "JMNMF-RHW7P-DMY6X-RF3DR-X2BQT" },
              
            { WindowsProductName.Windows_7_Professional, "FJ82H-XT6CR-J8D7P-XQJJ2-GPDD4" },
            { WindowsProductName.Windows_7_Professional_N, "MRPKT-YTG23-K7D7T-X2JMM-QY7MG" },
            { WindowsProductName.Windows_7_Professional_E, "W82YF-2Q76Y-63HXB-FGJG9-GF7QX" },
            { WindowsProductName.Windows_7_Enterprise, "33PXH-7Y6KF-2VJC9-XBBR8-HVTHH" },
            { WindowsProductName.Windows_7_Enterprise_N, "YDRBP-3D83W-TY26F-D46B2-XCKRJ" },
            { WindowsProductName.Windows_7_Enterprise_E, "C29WB-22CC8-VJ326-GHFJW-H9DH4" },
              
            { WindowsProductName.Windows_Vista_Business, "YFKBB-PQJJV-G996G-VWGXY-2V3X8" },
            { WindowsProductName.Windows_Vista_Business_N, "HMBQG-8H2RH-C77VX-27R82-VMQBT" },
            { WindowsProductName.Windows_Vista_Enterprise, "VKK3X-68KWM-X2YGT-QR4M6-4BWMV" },
            { WindowsProductName.Windows_Vista_Enterprise_N, "VTC42-BM838-43QHV-84HX6-XJXKV" },
              
            { WindowsProductName.Windows_Server_2022_Standard, "VDYBN-27WPP-V4HQT-9VMD4-VMK7H" },
            { WindowsProductName.Windows_Server_2022_Datacenter, "WX4NM-KYWYW-QJJR4-XV3QB-6VM33" },
              
            { WindowsProductName.Windows_Server_2019_Standard, "N69G4-B89J2-4G8F4-WWYCC-J464C" },
            { WindowsProductName.Windows_Server_2019_Datacenter, "WMDGN-G9PQG-XVVXX-R3X43-63DFG" },
              
            { WindowsProductName.Windows_Server_2016_Standard, "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY" },
            { WindowsProductName.Windows_Server_2016_Datacenter, "CB7KF-BWN84-R7R2Y-793K2-8XDDG" },
              
            { WindowsProductName.Windows_Server_2012_R2_Standard, "D2N9P-3P6X9-2R39C-7RTCD-MDVJX" },
            { WindowsProductName.Windows_Server_2012_R2_Datacenter, "W3GGN-FT8W3-Y4M27-J84CP-Q3VJ9"},
              
            { WindowsProductName.Windows_Server_2008_R2_Standard, "YC6KT-GKW9T-YTKYR-T4X34-R7VHC" },
            { WindowsProductName.Windows_Server_2008_R2_Datacenter, "74YFP-3QFB3-KQT8W-PMXWJ-7M648" },
            { WindowsProductName.Windows_Server_2008_R2_Enterprise, "489J6-VHDMP-X63PK-3K798-CPX3Y" }
        };

        public static Dictionary<WindowsProductName, string> MultipleActivationKeys { get; }
    }
}
