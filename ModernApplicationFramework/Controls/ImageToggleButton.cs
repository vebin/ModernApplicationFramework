﻿using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ModernApplicationFramework.Controls
{
    public class ImageToggleButton : ToggleButton
    {
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register(
            "HoverImage", typeof (ImageSource), typeof (ImageToggleButton), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty NormalImageProperty = DependencyProperty.Register(
            "NormalImage", typeof (ImageSource), typeof (ImageToggleButton), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register(
            "PressedImage", typeof (ImageSource), typeof (ImageToggleButton), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty CheckedImageProperty = DependencyProperty.Register(
            "CheckedImage", typeof (ImageSource), typeof (ImageToggleButton), new PropertyMetadata(default(ImageSource)));


        static ImageToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageToggleButton),
                new FrameworkPropertyMetadata(typeof(ImageToggleButton)));
        }

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        public ImageSource CheckedImage
        {
            get { return (ImageSource)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }

    }
}
