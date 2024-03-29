﻿/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Controls
{
	public class LayoutAnchorablePaneGroupControl : LayoutGridControl<ILayoutAnchorablePane>
	{
		private readonly LayoutAnchorablePaneGroup _model;

		internal LayoutAnchorablePaneGroupControl(LayoutAnchorablePaneGroup model)
			: base(model, model.Orientation)
		{
			_model = model;
		}

		protected override void OnFixChildrenDockLengths()
		{
			#region Setup DockWidth/Height for children

			if (_model.Orientation == Orientation.Horizontal)
			{
				foreach (
					var childModel in
						_model.Children.OfType<ILayoutPositionableElement>().Where(childModel => !childModel.DockWidth.IsStar))
				{
					childModel.DockWidth = new GridLength(1.0, GridUnitType.Star);
				}
			}
			else
			{
				foreach (
					var childModel in
						_model.Children.Select(t => t as ILayoutPositionableElement).Where(childModel => !childModel.DockHeight.IsStar))
				{
					childModel.DockHeight = new GridLength(1.0, GridUnitType.Star);
				}
			}

			#endregion
		}
	}
}