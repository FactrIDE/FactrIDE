using Gemini.Framework;
using Gemini.Framework.Services;

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Xceed.Wpf.AvalonDock.Layout;

namespace FactrIDE.Gemini.Modules.Startup.ViewModels
{
    // https://github.com/tgjones/gemini/issues/281
    [Export(typeof(ILayoutUpdateStrategy))]
    public class HackedLayoutInitializer : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            if (anchorableToShow.Content is ITool tool)
            {
                var preferredLocation = tool.PreferredLocation;
                string paneName = GetPaneName(preferredLocation);
                var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == paneName);
                if (toolsPane == null)
                {
                    toolsPane = preferredLocation switch
                    {
                        PaneLocation.Left => CreateAnchorablePane(layout, Orientation.Horizontal, paneName, InsertPosition.Start),
                        PaneLocation.Right => CreateAnchorablePane(layout, Orientation.Horizontal, paneName, InsertPosition.End),
                        PaneLocation.Bottom => CreateAnchorablePane(layout, Orientation.Vertical, paneName, InsertPosition.End),
                        _ => throw new ArgumentOutOfRangeException(),
                    };
                }
                toolsPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;
        }

        private static string GetPaneName(PaneLocation location) => location switch
        {
            PaneLocation.Left => "LeftPane",
            PaneLocation.Right => "RightPane",
            PaneLocation.Bottom => "BottomPane",
            _ => throw new ArgumentOutOfRangeException(nameof(location)),
        };

        private static LayoutAnchorablePane CreateAnchorablePane(LayoutRoot layout, Orientation orientation, string paneName, InsertPosition position)
        {
            var parent = layout.Descendents().OfType<LayoutPanel>().FirstOrDefault(d => d.Orientation == orientation);
            if (parent == null)
            {
                var newLayoutPanel = new LayoutPanel { Orientation = orientation };
                if (layout.RootPanel != null)
                {
                    newLayoutPanel.Children.Add(layout.RootPanel);
                }

                layout.RootPanel = newLayoutPanel;
                parent = newLayoutPanel;
            }

            var toolsPane = new LayoutAnchorablePane { Name = paneName };
            if (position == InsertPosition.Start)
                parent.InsertChildAt(0, toolsPane);
            else
                parent.Children.Add(toolsPane);
            return toolsPane;
        }

        private enum InsertPosition { Start, End }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            // If this is the first anchorable added to this pane, then use the preferred size.
            if (anchorableShown.Content is ITool tool && anchorableShown.Parent is LayoutAnchorablePane anchorablePane && anchorablePane.ChildrenCount == 1)
            {
                switch (tool.PreferredLocation)
                {
                    case PaneLocation.Left:
                    case PaneLocation.Right:
                        anchorablePane.DockWidth = new GridLength(tool.PreferredWidth, GridUnitType.Pixel);
                        break;
                    case PaneLocation.Bottom:
                        anchorablePane.DockHeight = new GridLength(tool.PreferredHeight, GridUnitType.Pixel);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer) => false;

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown) { }
    }
}