using System;
using System.Collections.Generic;

namespace Activator
{
    /// <summary>
    /// <para>本类用于存放一系列 Office 激活密钥的字典等内容</para>
    /// <para>This class is used to store a series of dictionaries of Office activation keys and other content</para>
    /// </summary>
    public class OfficeActivationKey
    {
        /// <summary>
        /// <para>支持的 Office 的专业增强版（即Pro Plus/LTSC 版）的批量在线激活密钥</para>
        /// <para>Volume online activation key for supported Office Pro Plus/LTSC editions</para>
        /// </summary>
        public static Dictionary<OfficeEditionName, string> OnlineActivationKey { get; } = new Dictionary<OfficeEditionName, string>()
        {
            { OfficeEditionName.Office_2021, "FXYTK-NJJ8C-GB6DW-3DYQT-6F7TH" },
            { OfficeEditionName.Office_2019, "NMMKJ-6RK4F-KMJVX-8D9MJ-6MWKP" },
            { OfficeEditionName.Office_2016, "XQNVK-8JYDB-WJ9W3-YJ8YR-WFG99" },
            { OfficeEditionName.Office_2013, "YC7DK-G2NP3-2QQC3-J6H88-GVGXT" },
            { OfficeEditionName.Office_2010, "VYBBJ-TRJPB-QFQRF-QFT4D-H3GVB" }
        };
    }
}
