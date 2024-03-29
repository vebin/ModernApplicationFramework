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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Controls
{
	public class LayoutPanelControl : LayoutGridControl<ILayoutPanelElement>
	{
		private readonly LayoutPanel _model;

		internal LayoutPanelControl(LayoutPanel model)
			: base(model, model.Orientation)
		{
			_model = model;
		}

		protected override void OnFixChildrenDockLengths()
		{
			if (ActualWidth == 0.0 ||
			    ActualHeight == 0.0)
				return;
			if (_model.Orientation == Orientation.Horizontal)
			{
				if (_model.ContainsChildOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>())
				{
					foreach (ILayoutPanelElement t in _model.Children)
					{
						var childContainerModel = t as ILayoutContainer;
						var childPositionableModel = t as ILayoutPositionableElement;

						if (childContainerModel != null &&
						    (childContainerModel.IsOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>() ||
						     childContainerModel.ContainsChildOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>()))
						{
							if (childPositionableModel != null)
								childPositionableModel.DockWidth = new GridLength(1.0, GridUnitType.Star);
						}
						else if (childPositionableModel != null && childPositionableModel.DockWidth.IsStar)
						{
							var childPositionableModelWidthActualSize = childPositionableModel as ILayoutPositionableElementWithActualSize;

							if (childPositionableModelWidthActualSize == null)
								continue;
							var widthToSet = Math.Max(childPositionableModelWidthActualSize.ActualWidth, childPositionableModel.DockMinWidth);

							widthToSet = Math.Min(widthToSet, ActualWidth/2.0);
							widthToSet = Math.Max(widthToSet, childPositionableModel.DockMinWidth);

							childPositionableModel.DockWidth = new GridLength(
								widthToSet,
								GridUnitType.Pixel);
						}
					}
				}
				else
				{
					foreach (
						var childPositionableModel in
							_model.Children.OfType<ILayoutPositionableElement>()
								.Where(childPositionableModel => !childPositionableModel.DockWidth.IsStar))
					{
						childPositionableModel.DockWidth = new GridLength(1.0, GridUnitType.Star);
					}
				}
			}
			else
			{
				if (_model.ContainsChildOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>())
				{
					foreach (ILayoutPanelElement t in _model.Children)
					{
						var childContainerModel = t as ILayoutContainer;
						var childPositionableModel = t as ILayoutPositionableElement;

						if (childContainerModel != null &&
						    (childContainerModel.IsOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>() ||
						     childContainerModel.ContainsChildOfType<LayoutDocumentPane, LayoutDocumentPaneGroup>()))
						{
							if (childPositionableModel != null)
								childPositionableModel.DockHeight = new GridLength(1.0, GridUnitType.Star);
						}
						else if (childPositionableModel != null && childPositionableModel.DockHeight.IsStar)
						{
							var childPositionableModelWidthActualSize = childPositionableModel as ILayoutPositionableElementWithActualSize;

							if (childPositionableModelWidthActualSize != null)
							{
								var heightToSet = Math.Max(childPositionableModelWidthActualSize.ActualHeight,
									childPositionableModel.DockMinHeight);
								heightToSet = Math.Min(heightToSet, ActualHeight/2.0);
								heightToSet = Math.Max(heightToSet, childPositionableModel.DockMinHeight);

								childPositionableModel.DockHeight = new GridLength(heightToSet, GridUnitType.Pixel);
							}
						}
					}
				}
				else
				{
					foreach (
						var childPositionableModel in
							_model.Children.Select(t => t as ILayoutPositionableElement)
								.Where(childPositionableModel => !childPositionableModel.DockHeight.IsStar))
					{
						childPositionableModel.DockHeight = new GridLength(1.0, GridUnitType.Star);
					}
				}
			}
		}
	}
}