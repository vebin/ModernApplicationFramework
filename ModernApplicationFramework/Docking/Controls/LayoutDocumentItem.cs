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
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Controls
{
	public class LayoutDocumentItem : LayoutItem
	{
		private LayoutDocument _document;

		internal LayoutDocumentItem()
		{
		}

		public string Description
		{
			get { return (string) GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		protected override void Close()
		{
			var dockingManager = _document.Root.Manager;
			dockingManager._ExecuteCloseCommand(_document);
		}

		protected virtual void OnDescriptionChanged(DependencyPropertyChangedEventArgs e)
		{
			_document.Description = (string) e.NewValue;
		}

		internal override void Attach(LayoutContent model)
		{
			_document = model as LayoutDocument;
			base.Attach(model);
		}

		internal override void Detach()
		{
			_document = null;
			base.Detach();
		}

		private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LayoutDocumentItem) d).OnDescriptionChanged(e);
		}

		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register("Description", typeof (string), typeof (LayoutDocumentItem),
				new FrameworkPropertyMetadata(null, OnDescriptionChanged));
	}
}