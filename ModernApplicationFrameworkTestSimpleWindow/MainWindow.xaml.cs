﻿using System;
using System.Windows.Media.Imaging;
using ModernApplicationFramework.ViewModels;

namespace ModernApplicationFrameworkTestSimpleWindow
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ((MainWindowViewModel) DataContext).UseStatusBar = false;
            ((MainWindowViewModel) DataContext).UseTitleBar = false;
            ((MainWindowViewModel) DataContext).IsSimpleWindow = true;
            ((MainWindowViewModel) DataContext).UseSimpleMovement = true;
        }
    }
}
