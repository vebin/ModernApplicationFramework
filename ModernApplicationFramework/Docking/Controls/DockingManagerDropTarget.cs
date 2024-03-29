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
using System.Windows.Media;
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Controls
{
	internal class DockingManagerDropTarget : DropTarget<DockingManager>
	{
		private readonly DockingManager _manager;

		internal DockingManagerDropTarget(DockingManager manager, Rect detectionRect, DropTargetType type)
			: base(manager, detectionRect, type)
		{
			_manager = manager;
		}

		public override Geometry GetPreviewPath(OverlayWindow overlayWindow, LayoutFloatingWindow floatingWindowModel)
		{
			var anchorableFloatingWindowModel = floatingWindowModel as LayoutAnchorableFloatingWindow;
			if (anchorableFloatingWindowModel != null)
			{
				var layoutAnchorablePane = anchorableFloatingWindowModel.RootPanel as ILayoutPositionableElement;
				var layoutAnchorablePaneWithActualSize =
					anchorableFloatingWindowModel.RootPanel as ILayoutPositionableElementWithActualSize;

				var targetScreenRect = TargetElement.GetScreenArea();

				switch (Type)
				{
					case DropTargetType.DockingManagerDockLeft:
					{
						var desideredWidth = layoutAnchorablePane.DockWidth.IsAbsolute
							? layoutAnchorablePane.DockWidth.Value
							: layoutAnchorablePaneWithActualSize.ActualWidth;
						var previewBoxRect = new Rect(
							targetScreenRect.Left - overlayWindow.Left,
							targetScreenRect.Top - overlayWindow.Top,
							Math.Min(desideredWidth, targetScreenRect.Width/2.0),
							targetScreenRect.Height);

						return new RectangleGeometry(previewBoxRect);
					}
					case DropTargetType.DockingManagerDockTop:
					{
						var desideredHeight = layoutAnchorablePane.DockHeight.IsAbsolute
							? layoutAnchorablePane.DockHeight.Value
							: layoutAnchorablePaneWithActualSize.ActualHeight;
						var previewBoxRect = new Rect(
							targetScreenRect.Left - overlayWindow.Left,
							targetScreenRect.Top - overlayWindow.Top,
							targetScreenRect.Width,
							Math.Min(desideredHeight, targetScreenRect.Height/2.0));

						return new RectangleGeometry(previewBoxRect);
					}
					case DropTargetType.DockingManagerDockRight:
					{
						var desideredWidth = layoutAnchorablePane.DockWidth.IsAbsolute
							? layoutAnchorablePane.DockWidth.Value
							: layoutAnchorablePaneWithActualSize.ActualWidth;
						var previewBoxRect = new Rect(
							targetScreenRect.Right - overlayWindow.Left - Math.Min(desideredWidth, targetScreenRect.Width/2.0),
							targetScreenRect.Top - overlayWindow.Top,
							Math.Min(desideredWidth, targetScreenRect.Width/2.0),
							targetScreenRect.Height);

						return new RectangleGeometry(previewBoxRect);
					}
					case DropTargetType.DockingManagerDockBottom:
					{
						var desideredHeight = layoutAnchorablePane.DockHeight.IsAbsolute
							? layoutAnchorablePane.DockHeight.Value
							: layoutAnchorablePaneWithActualSize.ActualHeight;
						var previewBoxRect = new Rect(
							targetScreenRect.Left - overlayWindow.Left,
							targetScreenRect.Bottom - overlayWindow.Top - Math.Min(desideredHeight, targetScreenRect.Height/2.0),
							targetScreenRect.Width,
							Math.Min(desideredHeight, targetScreenRect.Height/2.0));

						return new RectangleGeometry(previewBoxRect);
					}
				}
			}
			throw new InvalidOperationException();
		}

		protected override void Drop(LayoutAnchorableFloatingWindow floatingWindow)
		{
			switch (Type)
			{
				case DropTargetType.DockingManagerDockLeft:
				{
					if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Horizontal &&
					    _manager.Layout.RootPanel.Children.Count == 1)
						_manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

					if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Horizontal)
					{
						var layoutAnchorablePaneGroup = floatingWindow.RootPanel;
						if (layoutAnchorablePaneGroup != null &&
						    layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Horizontal)
						{
							var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
							for (int i = 0; i < childrenToTransfer.Length; i++)
								_manager.Layout.RootPanel.Children.Insert(i, childrenToTransfer[i]);
						}
						else
							_manager.Layout.RootPanel.Children.Insert(0, floatingWindow.RootPanel);
					}
					else
					{
						var newOrientedPanel = new LayoutPanel()
						{
							Orientation = System.Windows.Controls.Orientation.Horizontal
						};

						newOrientedPanel.Children.Add(floatingWindow.RootPanel);
						newOrientedPanel.Children.Add(_manager.Layout.RootPanel);

						_manager.Layout.RootPanel = newOrientedPanel;
					}
				}
					break;
				case DropTargetType.DockingManagerDockRight:
				{
					if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Horizontal &&
					    _manager.Layout.RootPanel.Children.Count == 1)
						_manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

					if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Horizontal)
					{
						var layoutAnchorablePaneGroup = floatingWindow.RootPanel;
						if (layoutAnchorablePaneGroup != null &&
						    layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Horizontal)
						{
							var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
							foreach (ILayoutAnchorablePane t in childrenToTransfer)
								_manager.Layout.RootPanel.Children.Add(t);
						}
						else
							_manager.Layout.RootPanel.Children.Add(floatingWindow.RootPanel);
					}
					else
					{
						var newOrientedPanel = new LayoutPanel
						{
							Orientation = System.Windows.Controls.Orientation.Horizontal
						};

						newOrientedPanel.Children.Add(_manager.Layout.RootPanel);
						newOrientedPanel.Children.Add(floatingWindow.RootPanel);

						_manager.Layout.RootPanel = newOrientedPanel;
					}
				}
					break;
				case DropTargetType.DockingManagerDockTop:
				{
					if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Vertical &&
					    _manager.Layout.RootPanel.Children.Count == 1)
						_manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Vertical;

					if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Vertical)
					{
						var layoutAnchorablePaneGroup = floatingWindow.RootPanel;
						if (layoutAnchorablePaneGroup != null &&
						    layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Vertical)
						{
							var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
							for (int i = 0; i < childrenToTransfer.Length; i++)
								_manager.Layout.RootPanel.Children.Insert(i, childrenToTransfer[i]);
						}
						else
							_manager.Layout.RootPanel.Children.Insert(0, floatingWindow.RootPanel);
					}
					else
					{
						var newOrientedPanel = new LayoutPanel()
						{
							Orientation = System.Windows.Controls.Orientation.Vertical
						};

						newOrientedPanel.Children.Add(floatingWindow.RootPanel);
						newOrientedPanel.Children.Add(_manager.Layout.RootPanel);

						_manager.Layout.RootPanel = newOrientedPanel;
					}
				}
					break;
				case DropTargetType.DockingManagerDockBottom:
				{
					if (_manager.Layout.RootPanel.Orientation != System.Windows.Controls.Orientation.Vertical &&
					    _manager.Layout.RootPanel.Children.Count == 1)
						_manager.Layout.RootPanel.Orientation = System.Windows.Controls.Orientation.Vertical;

					if (_manager.Layout.RootPanel.Orientation == System.Windows.Controls.Orientation.Vertical)
					{
						var layoutAnchorablePaneGroup = floatingWindow.RootPanel;
						if (layoutAnchorablePaneGroup != null &&
						    layoutAnchorablePaneGroup.Orientation == System.Windows.Controls.Orientation.Vertical)
						{
							var childrenToTransfer = layoutAnchorablePaneGroup.Children.ToArray();
							foreach (ILayoutAnchorablePane t in childrenToTransfer)
								_manager.Layout.RootPanel.Children.Add(t);
						}
						else
							_manager.Layout.RootPanel.Children.Add(floatingWindow.RootPanel);
					}
					else
					{
						var newOrientedPanel = new LayoutPanel()
						{
							Orientation = System.Windows.Controls.Orientation.Vertical
						};

						newOrientedPanel.Children.Add(_manager.Layout.RootPanel);
						newOrientedPanel.Children.Add(floatingWindow.RootPanel);

						_manager.Layout.RootPanel = newOrientedPanel;
					}
				}
					break;
			}
			base.Drop(floatingWindow);
		}
	}
}