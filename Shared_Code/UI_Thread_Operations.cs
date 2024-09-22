using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KMS_Activator
{
    internal static class UI_Thread_Operations
    {
        internal static void ShiftAwaitingAnimationEffectsTalker(int current)
        {
            if (Shared.mainWindow != null)
            {
                Shared.mainWindow.Dispatcher.Invoke
                (
                    () => { Shared.mainWindow.ShiftAwaitingAnimationEffects(current); }
                );
            }
        }
    }
}
