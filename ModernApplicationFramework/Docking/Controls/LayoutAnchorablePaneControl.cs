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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Controls
{
	public class LayoutAnchorablePaneControl : TabControl, ILayoutControl //, ILogicalChildrenContainer
	{
		private readonly LayoutAnchorablePane _model;

		public LayoutAnchorablePaneControl(LayoutAnchorablePane model)
		{
			if (model == null)
				throw new ArgumentNullException("model");

			_model = model;

			SetBinding(ItemsSourceProperty, new Binding("Model.Children") {Source = this});
			SetBinding(FlowDirectionProperty, new Binding("Model.Root.Manager.FlowDirection") {Source = this});

			LayoutUpdated += OnLayoutUpdated;
		}

		static LayoutAnchorablePaneControl()
		{
			FocusableProperty.OverrideMetadata(typeof (LayoutAnchorablePaneControl), new FrameworkPropertyMetadata(false));
		}

		public ILayoutElement Model => _model;

		protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			_model.SelectedContent.IsActive = true;

			base.OnGotKeyboardFocus(e);
		}

		protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);

			if (!e.Handled && _model.SelectedContent != null)
				_model.SelectedContent.IsActive = true;
		}

		protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseRightButtonDown(e);

			if (!e.Handled && _model.SelectedContent != null)
				_model.SelectedContent.IsActive = true;
		}

		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			var modelWithAtcualSize = _model as ILayoutPositionableElementWithActualSize;
			modelWithAtcualSize.ActualWidth = ActualWidth;
			modelWithAtcualSize.ActualHeight = ActualHeight;
		}
	}
}