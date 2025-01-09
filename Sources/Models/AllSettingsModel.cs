using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activator.Models
{
    public class AllSettingsModel
    {
        public static AllSettingsModel Instance { get; set; } = new AllSettingsModel();

        public bool IsWindowsActivationMode { get; set; } = true;

        public bool IsOfficeActivationMode { get; set; } = false;

        public int SelectedServerIndex { get; set; } = 0;

        public bool IsUpdateCheckEnabled { get; set; } = true;
    }
}
