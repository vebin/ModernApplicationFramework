﻿using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace ModernApplicationFramework.Controls
{
    internal sealed class MainWindowTitleBar : Border, INonClientArea
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new MainWindowTitleBarAutomationPeer(this);
        }

		//Not Needed anymore since WindowChrome is Doing the Job now
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            //if (e.Handled)
            //    return;
            //var source = PresentationSource.FromVisual(this) as HwndSource;
            //if (source != null)
            //    ModernChromeWindow.ShowWindowMenu(source, this, Mouse.GetPosition(this), RenderSize);
            //e.Handled = true;
        }

        public int HitTest(Point point)
        {
            return 2;
        }
    }
}
