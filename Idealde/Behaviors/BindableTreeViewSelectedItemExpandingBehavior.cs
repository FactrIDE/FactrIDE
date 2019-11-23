﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Idealde.Framework.Behaviors
{
    // From http://stackoverflow.com/a/20636049/208817
    public class BindableTreeViewSelectedItemExpandingBehavior : Behavior<TreeView>
    {
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(BindableTreeViewSelectedItemExpandingBehavior),
            new UIPropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            static void selectTreeViewItem(TreeViewItem tvi2)
            {
                if (tvi2 != null)
                {
                    tvi2.IsSelected = true;
                    tvi2.Focus();
                }
            }

            var tvi = e.NewValue as TreeViewItem;

            if (tvi == null)
            {
                var tree = ((BindableTreeViewSelectedItemExpandingBehavior) sender).AssociatedObject;
                if (tree == null)
                    return;

                if (!tree.IsLoaded)
                {
                    void handler(object sender2, RoutedEventArgs e2)
                    {
                        tvi = GetTreeViewItem(tree, e.NewValue);
                        selectTreeViewItem(tvi);
                        tree.Loaded -= handler;
                    }

                    tree.Loaded += handler;

                    return;
                }
                tvi = GetTreeViewItem(tree, e.NewValue);
            }

            selectTreeViewItem(tvi);
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }

        private static TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the 
                // virtualizing case even if the item is marked 
                // expanded we still need to do this step in order to 
                // regenerate the visuals because they may have been virtualized away.

                container.ApplyTemplate();
                var itemsPresenter =
                    (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter, 
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();
                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                var itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

                // Ensure that the generator for this panel has been created.
                _ = itemsHostPanel.Children;

                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    var subContainer = (TreeViewItem)container.ItemContainerGenerator.
                        ContainerFromIndex(i);
                    if (subContainer == null)
                    {
                        continue;
                    }

                    // Search the next level for the object.
                    var resultContainer = GetTreeViewItem(subContainer, item);
                    if (resultContainer != null)
                    {
                        return resultContainer;
                    }
                    else
                    {
                        // The object is not under this TreeViewItem
                        // so collapse it.
                        //subContainer.IsExpanded = false;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <returns></returns>
        private static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                if (VisualTreeHelper.GetChild(visual, i) is Visual child)
                {
                    if (child is T correctlyTyped)
                        return correctlyTyped;

                    if (FindVisualChild<T>(child) is T result)
                        return result;
                }
            }

            return null;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
                AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }
    }
}