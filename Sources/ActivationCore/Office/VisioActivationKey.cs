using System;
using System.Collections.Generic;

namespace Activator
{
    /// <summary>
    /// <para>本类用于存放一系列 Visio 激活密钥的字典等内容</para>
    /// <para>This class is used to store a series of dictionaries of Visio activation keys and other content</para>
    /// </summary>
    public class VisioActivationKey
    {
        /// <summary>
        /// <para>支持的 Visio 的专业增强版（即Pro Plus/LTSC 版）的批量在线激活密钥</para>
        /// <para>Volume online activation key for supported Visio Pro Plus/LTSC editions</para>
        /// </summary>
        public static Dictionary<VisioEditionName, string> OnlineActivationKey { get; } = new Dictionary<VisioEditionName, string>()
        {
            { VisioEditionName.Visio_2021, "KNH8D-FGHT4-T8RK3-CTDYJ-K2HT4" },
            { VisioEditionName.Visio_2019, "9BGNQ-K37YR-RQHF2-38RQ3-7VCBB" },
            { VisioEditionName.Visio_2016, "PD3PC-RHNGV-FXJ29-8JK7D-RJRJK" },
            { VisioEditionName.Visio_2013, "C2FG9-N6J68-H8BTJ-BW3QX-RM3B3" },
            { VisioEditionName.Visio_2010, "7MCW8-VRQVK-G677T-PDJCM-Q8TCP" }
        };
    }
}
