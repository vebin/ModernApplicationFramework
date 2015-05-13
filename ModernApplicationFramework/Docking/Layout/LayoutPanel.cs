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
using System.Windows.Controls;
using System.Windows.Markup;

namespace ModernApplicationFramework.Docking.Layout
{
	[ContentProperty("Children")]
	[Serializable]
	public class LayoutPanel : LayoutPositionableGroup<ILayoutPanelElement>, ILayoutPanelElement, ILayoutOrientableGroup
	{
		private Orientation _orientation;

		public LayoutPanel()
		{
		}

		public LayoutPanel(ILayoutPanelElement firstChild)
		{
			Children.Add(firstChild);
		}

		public Orientation Orientation
		{
			get { return _orientation; }
			set
			{
				if (_orientation == value)
					return;
				RaisePropertyChanging("Orientation");
				_orientation = value;
				RaisePropertyChanged("Orientation");
			}
		}

#if TRACE
		public override void ConsoleDump(int tab)
		{
			System.Diagnostics.Trace.Write(new string(' ', tab*4));
			System.Diagnostics.Trace.WriteLine(string.Format("Panel({0})", Orientation));

			foreach (var layoutPanelElement in Children)
			{
				var child = (LayoutElement) layoutPanelElement;
				child.ConsoleDump(tab + 1);
			}
		}
#endif

		public override void ReadXml(System.Xml.XmlReader reader)
		{
			if (reader.MoveToAttribute("Orientation"))
				Orientation = (Orientation) Enum.Parse(typeof (Orientation), reader.Value, true);
			base.ReadXml(reader);
		}

		public override void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteAttributeString("Orientation", Orientation.ToString());
			base.WriteXml(writer);
		}

		protected override bool GetVisibility()
		{
			return Children.Any(c => c.IsVisible);
		}
	}
}