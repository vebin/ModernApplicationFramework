﻿using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using ModernApplicationFramework.Controls;

namespace ModernApplicationFramework.Docking.Controls
{
	sealed class DragUndockHeader : Border, INonClientArea
	{

		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			return new PointHitTestResult(this, hitTestParameters.HitPoint);
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MainWindowTitleBarAutomationPeer(this);
		}

		int INonClientArea.HitTest(Point point)
		{
			return 2;
		}

		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			if (e.Handled)
				return;
			var source = PresentationSource.FromVisual(this) as HwndSource;
			if (source != null)
				ModernChromeWindow.ShowWindowMenu(source, this, Mouse.GetPosition(this), RenderSize);
			e.Handled = true;
		}
	}
}
