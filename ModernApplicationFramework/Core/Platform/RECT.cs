﻿using System;
using System.Windows;

namespace ModernApplicationFramework.Core.Platform
{
    [Serializable]
    // ReSharper disable once InconsistentNaming
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public System.Windows.Point Position
        {
            get { return new System.Windows.Point(Left, Top); }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = Top + value; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = Left + value; }
        }

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(Rect rect)
        {
            Left = (int)rect.Left;
            Top = (int)rect.Top;
            Right = (int)rect.Right;
            Bottom = (int)rect.Bottom;
        }

        public void Offset(int dx, int dy)
        {
            Left += dx;
            Right += dx;
            Top += dy;
            Bottom += dy;
        }

        public Int32Rect ToInt32Rect()
        {
            return new Int32Rect(Left, Top, Width, Height);
        }
    }
}