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
using System.Windows.Media;
using ModernApplicationFramework.Docking.Layout;

namespace ModernApplicationFramework.Docking.Controls
{
	internal class DocumentPaneDropTarget : DropTarget<LayoutDocumentPaneControl>
	{
		private readonly int _tabIndex = -1;
		private readonly LayoutDocumentPaneControl _targetPane;

		internal DocumentPaneDropTarget(LayoutDocumentPaneControl paneControl, Rect detectionRect, DropTargetType type)
			: base(paneControl, detectionRect, type)
		{
			_targetPane = paneControl;
		}

		internal DocumentPaneDropTarget(LayoutDocumentPaneControl paneControl, Rect detectionRect, DropTargetType type,
			int tabIndex)
			: base(paneControl, detectionRect, type)
		{
			_targetPane = paneControl;
			_tabIndex = tabIndex;
		}

		public override Geometry GetPreviewPath(OverlayWindow overlayWindow, LayoutFloatingWindow floatingWindowModel)
		{
			switch (Type)
			{
				case DropTargetType.DocumentPaneDockInside:
				{
					var targetScreenRect = TargetElement.GetScreenArea();
					targetScreenRect.Offset(-overlayWindow.Left, -overlayWindow.Top);

					if (_tabIndex == -1)
					{
						return new RectangleGeometry(targetScreenRect);
					}
					else
					{
						var translatedDetectionRect = new Rect(DetectionRects[0].TopLeft, DetectionRects[0].BottomRight);
						translatedDetectionRect.Offset(-overlayWindow.Left, -overlayWindow.Top);

						var pathFigure = new PathFigure {StartPoint = targetScreenRect.BottomRight};
						pathFigure.Segments.Add(new LineSegment()
						{
							Point = new Point(targetScreenRect.Right, translatedDetectionRect.Bottom)
						});
						pathFigure.Segments.Add(new LineSegment() {Point = translatedDetectionRect.BottomRight});
						pathFigure.Segments.Add(new LineSegment() {Point = translatedDetectionRect.TopRight});
						pathFigure.Segments.Add(new LineSegment() {Point = translatedDetectionRect.TopLeft});
						pathFigure.Segments.Add(new LineSegment() {Point = translatedDetectionRect.BottomLeft});
						pathFigure.Segments.Add(new LineSegment()
						{
							Point = new Point(targetScreenRect.Left, translatedDetectionRect.Bottom)
						});
						pathFigure.Segments.Add(new LineSegment() {Point = targetScreenRect.BottomLeft});
						pathFigure.IsClosed = true;
						pathFigure.IsFilled = true;
						pathFigure.Freeze();
						return new PathGeometry(new[] {pathFigure});
					}
				}
				case DropTargetType.DocumentPaneDockBottom:
				{
					var targetScreenRect = TargetElement.GetScreenArea();
					targetScreenRect.Offset(-overlayWindow.Left, -overlayWindow.Top);
					targetScreenRect.Offset(0.0, targetScreenRect.Height/2.0);
					targetScreenRect.Height /= 2.0;
					return new RectangleGeometry(targetScreenRect);
				}
				case DropTargetType.DocumentPaneDockTop:
				{
					var targetScreenRect = TargetElement.GetScreenArea();
					targetScreenRect.Offset(-overlayWindow.Left, -overlayWindow.Top);
					targetScreenRect.Height /= 2.0;
					return new RectangleGeometry(targetScreenRect);
				}
				case DropTargetType.DocumentPaneDockLeft:
				{
					var targetScreenRect = TargetElement.GetScreenArea();
					targetScreenRect.Offset(-overlayWindow.Left, -overlayWindow.Top);
					targetScreenRect.Width /= 2.0;
					return new RectangleGeometry(targetScreenRect);
				}
				case DropTargetType.DocumentPaneDockRight:
				{
					var targetScreenRect = TargetElement.GetScreenArea();
					targetScreenRect.Offset(-overlayWindow.Left, -overlayWindow.Top);
					targetScreenRect.Offset(targetScreenRect.Width/2.0, 0.0);
					targetScreenRect.Width /= 2.0;
					return new RectangleGeometry(targetScreenRect);
				}
			}

			return null;
		}

		protected override void Drop(LayoutDocumentFloatingWindow floatingWindow)
		{
			ILayoutDocumentPane targetModel = _targetPane.Model as ILayoutDocumentPane;

			switch (Type)
			{
				case DropTargetType.DocumentPaneDockBottom:
				{
					var newLayoutDocumentPane = new LayoutDocumentPane(floatingWindow.RootDocument);
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup {Orientation = System.Windows.Controls.Orientation.Vertical};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
							newParentModel.Children.Add(newLayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Vertical)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Vertical;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex + 1, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Vertical
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(targetModel);
								newChildGroup.Children.Add(newLayoutDocumentPane);
							}
						}
					}
				}
					break;
				case DropTargetType.DocumentPaneDockTop:
				{
					var newLayoutDocumentPane = new LayoutDocumentPane(floatingWindow.RootDocument);
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Vertical};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
							newParentModel.Children.Insert(0, newLayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Vertical)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Vertical;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Vertical
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(newLayoutDocumentPane);
								newChildGroup.Children.Add(targetModel);
							}
						}
					}
				}
					break;
				case DropTargetType.DocumentPaneDockLeft:
				{
					var newLayoutDocumentPane = new LayoutDocumentPane(floatingWindow.RootDocument);
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Horizontal};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(targetModel);
							newParentModel.Children.Insert(0, newLayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Horizontal)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Horizontal;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Horizontal
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(newLayoutDocumentPane);
								newChildGroup.Children.Add(targetModel);
							}
						}
					}
				}
					break;
				case DropTargetType.DocumentPaneDockRight:
				{
					var newLayoutDocumentPane = new LayoutDocumentPane(floatingWindow.RootDocument);
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Horizontal};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
							newParentModel.Children.Add(newLayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Horizontal)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Horizontal;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex + 1, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Horizontal
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(targetModel);
								newChildGroup.Children.Add(newLayoutDocumentPane);
							}
						}
					}
				}
					break;
				case DropTargetType.DocumentPaneDockInside:
				{
					var paneModel = targetModel as LayoutDocumentPane;
					var sourceModel = floatingWindow.RootDocument;

					int i = _tabIndex == -1 ? 0 : _tabIndex;
					sourceModel.IsActive = false;
					paneModel?.Children.Insert(i, sourceModel);
					sourceModel.IsActive = true;
				}
					break;
			}

			base.Drop(floatingWindow);
		}

		protected override void Drop(LayoutAnchorableFloatingWindow floatingWindow)
		{
			ILayoutDocumentPane targetModel = _targetPane.Model as ILayoutDocumentPane;

			switch (Type)
			{
				case DropTargetType.DocumentPaneDockBottom:

					#region DropTargetType.DocumentPaneDockBottom

				{
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;
						var newLayoutDocumentPane = new LayoutDocumentPane();

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Vertical};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
							newParentModel.Children.Add(newLayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Vertical)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Vertical;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex + 1, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Vertical
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(targetModel);
								newChildGroup.Children.Add(newLayoutDocumentPane);
							}
						}

						foreach (var cntToTransfer in floatingWindow.RootPanel.Descendents().OfType<LayoutAnchorable>().ToArray())
							newLayoutDocumentPane.Children.Add(cntToTransfer);
					}
				}
					break;

					#endregion

				case DropTargetType.DocumentPaneDockTop:

					#region DropTargetType.DocumentPaneDockTop

				{
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;
						var newLayoutDocumentPane = new LayoutDocumentPane();

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Vertical};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(newLayoutDocumentPane);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Vertical)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Vertical;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Vertical
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(newLayoutDocumentPane);
								newChildGroup.Children.Add(targetModel);
							}
						}

						foreach (var cntToTransfer in floatingWindow.RootPanel.Descendents().OfType<LayoutAnchorable>().ToArray())
							newLayoutDocumentPane.Children.Add(cntToTransfer);
					}
				}
					break;

					#endregion

				case DropTargetType.DocumentPaneDockLeft:

					#region DropTargetType.DocumentPaneDockLeft

				{
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;
						var newLayoutDocumentPane = new LayoutDocumentPane();

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Horizontal};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(newLayoutDocumentPane);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Horizontal)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Horizontal;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Horizontal
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(newLayoutDocumentPane);
								newChildGroup.Children.Add(targetModel);
							}
						}

						foreach (var cntToTransfer in floatingWindow.RootPanel.Descendents().OfType<LayoutAnchorable>().ToArray())
							newLayoutDocumentPane.Children.Add(cntToTransfer);
					}
				}
					break;

					#endregion

				case DropTargetType.DocumentPaneDockRight:

					#region DropTargetType.DocumentPaneDockRight

				{
					if (targetModel != null)
					{
						var parentModel = targetModel.Parent as LayoutDocumentPaneGroup;
						var newLayoutDocumentPane = new LayoutDocumentPane();

						if (parentModel == null)
						{
							var parentContainer = targetModel.Parent;
							var newParentModel = new LayoutDocumentPaneGroup() {Orientation = System.Windows.Controls.Orientation.Horizontal};
							parentContainer.ReplaceChild(targetModel, newParentModel);
							newParentModel.Children.Add(targetModel as LayoutDocumentPane);
							newParentModel.Children.Add(newLayoutDocumentPane);
						}
						else
						{
							var manager = parentModel.Root.Manager;
							if (!manager.AllowMixedOrientation || parentModel.Orientation == System.Windows.Controls.Orientation.Horizontal)
							{
								parentModel.Orientation = System.Windows.Controls.Orientation.Horizontal;
								int targetPaneIndex = parentModel.IndexOfChild(targetModel);
								parentModel.Children.Insert(targetPaneIndex + 1, newLayoutDocumentPane);
							}
							else
							{
								LayoutDocumentPaneGroup newChildGroup = new LayoutDocumentPaneGroup
								{
									Orientation = System.Windows.Controls.Orientation.Horizontal
								};
								parentModel.ReplaceChild(targetModel, newChildGroup);
								newChildGroup.Children.Add(targetModel);
								newChildGroup.Children.Add(newLayoutDocumentPane);
							}
						}

						foreach (var cntToTransfer in floatingWindow.RootPanel.Descendents().OfType<LayoutAnchorable>().ToArray())
							newLayoutDocumentPane.Children.Add(cntToTransfer);
					}
				}
					break;

					#endregion

				case DropTargetType.DocumentPaneDockInside:

					#region DropTargetType.DocumentPaneDockInside

				{
					var paneModel = targetModel as LayoutDocumentPane;
					var layoutAnchorablePaneGroup = floatingWindow.RootPanel;

					int i = _tabIndex == -1 ? 0 : _tabIndex;
					LayoutAnchorable anchorableToActivate = null;
					foreach (var anchorableToImport in layoutAnchorablePaneGroup.Descendents().OfType<LayoutAnchorable>().ToArray())
					{
						paneModel?.Children.Insert(i, anchorableToImport);
						i++;
						anchorableToActivate = anchorableToImport;
					}

					if (anchorableToActivate != null) anchorableToActivate.IsActive = true;
				}
					break;

					#endregion
			}

			base.Drop(floatingWindow);
		}
	}
}