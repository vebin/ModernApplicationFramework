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
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ModernApplicationFramework.Docking.Controls
{
    public static class Extentions
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
	        if (depObj == null)
				yield break;
	        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
	        {
		        DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
		        var children = child as T;
		        if (children != null)
		        {
			        yield return children;
		        }

		        foreach (T childOfChild in FindVisualChildren<T>(child))
		        {
			        yield return childOfChild;
		        }
	        }
        }

	    public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject depObj) where T : DependencyObject
	    {
		    if (depObj == null)
				yield break;
		    foreach (DependencyObject child in LogicalTreeHelper.GetChildren(depObj).OfType<DependencyObject>())
		    {
			    var children = child as T;
			    if (children != null)
			    {
				    yield return children;
			    }

			    foreach (T childOfChild in FindLogicalChildren<T>(child))
			    {
				    yield return childOfChild;
			    }
		    }
	    }

	    public static DependencyObject FindVisualTreeRoot(this DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    // If we're in Logical Land then we must walk 
                    // up the logical tree until we find a 
                    // Visual/Visual3D to get us back to Visual Land.
                    current = LogicalTreeHelper.GetParent(current);
                }
            }

            return result;
        }

        public static T FindVisualAncestor<T>(this DependencyObject dependencyObject) where T : class
        {
            DependencyObject target = dependencyObject;
            do
            {
                target = VisualTreeHelper.GetParent(target);
            }
            while (target != null && !(target is T));
            return target as T;
        }

        public static T FindLogicalAncestor<T>(this DependencyObject dependencyObject) where T : class
        {
            DependencyObject target = dependencyObject;
            do
            {
                var current = target;
                target = LogicalTreeHelper.GetParent(target) ?? VisualTreeHelper.GetParent(current);
            }
            while (target != null && !(target is T));
            return target as T;
        }

    }
}
