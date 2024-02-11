using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using static KMS_Activator.Shared;
using System.Windows.Media;
using SysTimer = System.Timers;

namespace KMS_Activator
{
    /// <summary>
    ///     <para>
    ///         该类主要用以实现UI线程上的一些简单动画效果的功能
    ///     </para>
    ///     <para>
    ///         This class is mainly used to achieve some simple animation effects on the UI thread
    ///     </para>
    /// </summary>
    public static class Animations_Related
    {
        /// <summary>
        ///     <para>
        ///         该函数用以让 <see cref="List{Grid}"/> 集合中的所有Grid对象向左平移移出窗口左端
        ///     </para>
        ///     <para>
        ///         This function shifts all Grid objects in the <see cref="List{Grid}"/> collection beyond the left end of the window
        ///     </para>
        /// </summary>
        /// <param name="myGridList">
        ///     <para>
        ///         一个 <see cref="List{Grid}"/> 类型的集合
        ///     </para>
        ///     <para>
        ///         A collection of type <see cref="List{Grid}"/>
        ///     </para>
        /// </param>
        public static void MainW_Slide(List<Grid> myGridList)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            foreach (Grid myGrid in myGridList)
            {
                myGrid.RenderTransform = translateTransform;
            }

            // 创建平移动画
            // Create the panning animation
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = -mainWindow!.Width,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            // 添加慢入慢出的缓动函数
            // Add slow-in and slow-out easing functions
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            // 启动动画
            // Starting the animation
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        /// <summary>
        ///     <para>
        ///         该函数用以让 <see cref="List{Grid}"/> 集合中的所有Grid对象向右平移以复位回窗口中间
        ///     </para>
        ///     <para>
        ///         This function shifts all Grid objects in the <see cref="List{Grid}"/> collection to the right to reset them back to the middle of the window
        ///     </para>
        /// </summary>
        /// <param name="myGridList">
        ///     <para>
        ///         一个 <see cref="List{Grid}"/> 类型的集合
        ///     </para>
        ///     <para>
        ///         A collection of type <see cref="List{Grid}"/>
        ///     </para>
        /// </param>
        public static void MainW_SlideBack(List<Grid> myGridList)
        {
            // 同理MainW_Slide()函数
            // Same with MainW Slide()
            TranslateTransform translateTransform = new TranslateTransform();
            foreach (Grid myGrid in myGridList)
            {
                myGrid.RenderTransform = translateTransform;
            }
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = -mainWindow!.Width,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.4)
            };
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}
