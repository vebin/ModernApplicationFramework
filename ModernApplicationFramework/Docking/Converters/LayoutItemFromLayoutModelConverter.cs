﻿/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System;
using System.Windows.Data;
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Converters
{
    public class LayoutItemFromLayoutModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var layoutModel = value as LayoutContent;
	        if (layoutModel?.Root?.Manager == null)
                return null;

            var layoutItemModel = layoutModel.Root.Manager.GetLayoutItemFromModel(layoutModel);
            return layoutItemModel ?? Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
