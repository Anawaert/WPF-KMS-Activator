using System;
using System.Collections.Generic;

namespace Activator
{
    public class VisioActivationKey
    {
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
