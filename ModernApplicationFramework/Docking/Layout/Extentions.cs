﻿/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ModernApplicationFramework.Docking.Layout
{
    public static class Extensions
    {
        public static IEnumerable<ILayoutElement> Descendents(this ILayoutElement element)
        {
            var container = element as ILayoutContainer;
	        if (container == null)
				yield break;
	        foreach (var childElement in container.Children)
	        {
		        yield return childElement;
		        foreach (var childChildElement in childElement.Descendents())
			        yield return childChildElement;
	        }
        }

        public static T FindParent<T>(this ILayoutElement element) //where T : ILayoutContainer
        { 
            var parent = element.Parent;
            while (parent != null &&
                !(parent is T))
                parent = parent.Parent;
            return (T)parent;
        }

        public static ILayoutRoot GetRoot(this ILayoutElement element) //where T : ILayoutContainer
        {
	        var root = element as ILayoutRoot;
	        if (root != null)
                return root;

            var parent = element.Parent;
            while (parent != null &&
                !(parent is ILayoutRoot))
                parent = parent.Parent;

            return (ILayoutRoot)parent;
        }

        public static bool ContainsChildOfType<T>(this ILayoutContainer element)
        {
	        return element.Descendents().OfType<T>().Any();
        }

	    public static bool ContainsChildOfType<T, TS>(this ILayoutContainer container)
	    {
		    return container.Descendents().Any(childElement => childElement is T || childElement is TS);
	    }

	    public static bool IsOfType<T, TS>(this ILayoutContainer container)
        {
            return container is T || container is TS;
        }

        public static AnchorSide GetSide(this ILayoutElement element)
        {
            var parentContainer = element.Parent as ILayoutOrientableGroup;
            if (parentContainer != null)
            {
                if (!parentContainer.ContainsChildOfType<LayoutDocumentPaneGroup, LayoutDocumentPane>())
                    return GetSide(parentContainer);

                foreach (var childElement in parentContainer.Children)
                {
                    if (childElement == element ||
                        childElement.Descendents().Contains(element))
                        return parentContainer.Orientation == System.Windows.Controls.Orientation.Horizontal ?
                            AnchorSide.Left : AnchorSide.Top;

                    var childElementAsContainer = childElement as ILayoutContainer;
                    if (childElementAsContainer != null &&
                        (childElementAsContainer.IsOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>() ||
                        childElementAsContainer.ContainsChildOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>()))
                    {
                        return parentContainer.Orientation == System.Windows.Controls.Orientation.Horizontal ?
                           AnchorSide.Right : AnchorSide.Bottom;
                    }
                }
            }

            Debug.Fail("Unable to find the side for an element, possible layout problem!");
            return AnchorSide.Right;
        }


        internal static void KeepInsideNearestMonitor(this ILayoutElementForFloatingWindow paneInsideFloatingWindow)
        {
	        Win32Helper.RECT r = new Win32Helper.RECT
	        {
		        Left = (int) paneInsideFloatingWindow.FloatingLeft,
		        Top = (int) paneInsideFloatingWindow.FloatingTop
	        };
	        r.Bottom = r.Top + (int)paneInsideFloatingWindow.FloatingHeight;
            r.Right = r.Left + (int)paneInsideFloatingWindow.FloatingWidth;

            const uint monitorDefaulttonearest = 0x00000002;
            const uint monitorDefaulttonull = 0x00000000;

            System.IntPtr monitor = Win32Helper.MonitorFromRect(ref r, monitorDefaulttonull);
            if (monitor == System.IntPtr.Zero)
            {
                System.IntPtr nearestmonitor = Win32Helper.MonitorFromRect(ref r, monitorDefaulttonearest);
                if (nearestmonitor != System.IntPtr.Zero)
                {
                    Win32Helper.MonitorInfo monitorInfo = new Win32Helper.MonitorInfo();
                    monitorInfo.Size = Marshal.SizeOf(monitorInfo);
                    Win32Helper.GetMonitorInfo(nearestmonitor, monitorInfo);

                    if (paneInsideFloatingWindow.FloatingLeft < monitorInfo.Work.Left)
                    {
                        paneInsideFloatingWindow.FloatingLeft = monitorInfo.Work.Left + 10;
                    }

                    if (paneInsideFloatingWindow.FloatingLeft + paneInsideFloatingWindow.FloatingWidth > monitorInfo.Work.Right)
                    {
                        paneInsideFloatingWindow.FloatingLeft = monitorInfo.Work.Right - (paneInsideFloatingWindow.FloatingWidth + 10);
                    }

                    if (paneInsideFloatingWindow.FloatingTop < monitorInfo.Work.Top)
                    {
                        paneInsideFloatingWindow.FloatingTop = monitorInfo.Work.Top + 10;
                    }

                    if (paneInsideFloatingWindow.FloatingTop + paneInsideFloatingWindow.FloatingHeight > monitorInfo.Work.Bottom)
                    {
                        paneInsideFloatingWindow.FloatingTop = monitorInfo.Work.Bottom - (paneInsideFloatingWindow.FloatingHeight + 10);
                    }
                }
            }

        }

    }
}
