using System;
using System.Collections.Generic;

namespace Activator
{
    public class OfficeActivationKeys
    {
        public static Dictionary<OfficeProductName, string> OnlineActivationKeys { get; } = new Dictionary<OfficeProductName, string>()
        {
            { OfficeProductName.Office_2021, "FXYTK-NJJ8C-GB6DW-3DYQT-6F7TH" },
            { OfficeProductName.Office_2019, "NMMKJ-6RK4F-KMJVX-8D9MJ-6MWKP" },
            { OfficeProductName.Office_2016, "XQNVK-8JYDB-WJ9W3-YJ8YR-WFG99" },
            { OfficeProductName.Office_2013, "YC7DK-G2NP3-2QQC3-J6H88-GVGXT" },
            { OfficeProductName.Office_2010, "VYBBJ-TRJPB-QFQRF-QFT4D-H3GVB" }
        };
    }
}
