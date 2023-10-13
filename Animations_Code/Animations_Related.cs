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

namespace KMS_Activator
{
    public static class Animations_Related
    {
        public static void MainW_Slide(Grid myGrid)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            myGrid.RenderTransform = translateTransform;

            // 创建平移动画
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0; // 起始位置（左侧屏幕外）
            animation.To = -mainWindow.Width;     // 终止位置（屏幕中央）
            animation.Duration = TimeSpan.FromSeconds(0.66); // 动画持续时间

            // 添加慢入慢出的缓动函数
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            // 启动动画
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        public static void MainW_SlideBack(Grid myGrid)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            myGrid.RenderTransform = translateTransform;

            // 创建平移动画
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = -mainWindow.Width; // 起始位置（左侧屏幕外）
            animation.To = 0;     // 终止位置（屏幕中央）
            animation.Duration = TimeSpan.FromSeconds(0.66); // 动画持续时间

            // 添加慢入慢出的缓动函数
            animation.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

            // 启动动画
            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }
    }
}
