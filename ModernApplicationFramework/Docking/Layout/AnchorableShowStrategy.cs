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

namespace ModernApplicationFramework.Docking.Layout
{
    [Flags]
    public enum AnchorableShowStrategy : byte
    {
        Most  = 0x0001,
        Left  = 0x0002,
        Right = 0x0004,
        Top   = 0x0010,
        Bottom= 0x0020,
    }
}
