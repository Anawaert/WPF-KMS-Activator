using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using static KMS_Activator.Shared;
using System.Windows.Media;
using SysTimer = System.Timers;

namespace KMS_Activator
{
    public static class Animations_Related
    {
        public static void MainW_Slide(List<Grid> myGridList)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            foreach (Grid myGrid in myGridList)
            {
                myGrid.RenderTransform = translateTransform;
            }

            // 创建平移动画
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = -mainWindow.Width,
                Duration = TimeSpan.FromSeconds(0.4)
            };
            // 添加慢入慢出的缓动函数
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            // 启动动画
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        public static void MainW_SlideBack(List<Grid> myGridList)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            foreach (Grid myGrid in myGridList)
            {
                myGrid.RenderTransform = translateTransform;
            }

            // 创建平移动画
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = -mainWindow.Width,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            // 添加慢入慢出的缓动函数
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            // 启动动画
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}
