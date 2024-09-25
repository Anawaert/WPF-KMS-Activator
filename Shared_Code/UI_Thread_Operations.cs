using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KMS_Activator
{
    /// <summary>
    ///     <para>该类主要用以实现UI线程上的一些简单动画效果的功能</para>
    ///     <para>This class is mainly used to achieve some simple animation effects on the UI thread</para>
    /// </summary>
    internal static class UI_Thread_Operations
    {
        /// <summary>
        ///     <para>该函数主要通知UI线程改变激活过程中的加载动画效果</para>
        /// </summary>
        /// <param name="current">
        ///     <para>需要改变效果的步骤</para>
        /// </param>
        internal static void ShiftAwaitingAnimationEffectsTalker(int current)
        {
            Shared.mainWindow?.Dispatcher.Invoke
            (
                () => { Shared.mainWindow.ShiftAwaitingAnimationEffects(current); }
            );
        }
    }
}
