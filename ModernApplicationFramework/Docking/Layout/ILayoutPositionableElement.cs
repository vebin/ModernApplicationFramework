﻿/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System.Windows;

namespace ModernApplicationFramework.Docking.Layout
{
    internal interface ILayoutPositionableElement : ILayoutElement, ILayoutElementForFloatingWindow
    {
        GridLength DockWidth
        {
            get;
            set;
        }

        GridLength DockHeight
        {
            get;
            set;
        }

        double DockMinWidth { get; set; }
        double DockMinHeight { get; set; }



        bool IsVisible { get; }
    }


    internal interface ILayoutPositionableElementWithActualSize
    {
        double ActualWidth { get; set; }
        double ActualHeight { get; set; }
    }

    internal interface ILayoutElementForFloatingWindow
    {
        double FloatingWidth { get; set; }
        double FloatingHeight { get; set; }
        double FloatingLeft { get; set; }
        double FloatingTop { get; set; }
        bool IsMaximized { get; set; }
    }
}
