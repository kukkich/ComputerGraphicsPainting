using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Lab1.Models;
using Lab1.Models.Actions;
using Lab1.Models.Controls;
using Lab1.ViewModels;
using Microsoft.VisualBasic;
using SharpGL;
using SharpGL.WPF;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace Lab1
{
    public partial class MainWindow : Window
    {
        public PointsApp PointsApp { get; }
        public PointsAppView AppView { get; }
        public PointContext PointContext => PointsApp.PointContext;
        public InputControl InputControl => PointsApp.InputControl;

        public MainWindow()
        {
            InitializeComponent();

            AppView = new PointsAppView(GroupsTable, ColorPicker);
            PointsApp = new PointsApp(Dispatcher, AppView);


            DataContext = this;
        }

        private void OpenGLControl_OpenGLInitialized(object s, OpenGLRoutedEventArgs args)
        {
            PointsApp.InitOpenGL((OpenGLControl)s, args);
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            PointsApp.Render(args);
        }

        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            InputControl.OnMouseHover((OpenGLControl)sender, e);
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            InputControl.OnMouseLeave((OpenGLControl)sender, e);
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            InputControl.OnMouseEnter((OpenGLControl)sender, e);
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InputControl.OnLeftClick((OpenGLControl)sender, e);
        }

        private void UIElement_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InputControl.OnRightClick((OpenGLControl)sender, e);
        }

        private void OpenGLControl_Resized(object sender, OpenGLRoutedEventArgs args)
        {

        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Z)
            {
                PointsApp.InputControl.OnUndoButton();
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Y)
            {
                PointsApp.InputControl.OnRedoButton();
            }

            try
            {
                if (e.Key == Key.E)
                {
                    PointsApp.InputControl.OnEditModeToggle();
                }
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }


        }

        private void PointsGroups_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is PointsGroupView selectedItem)
            {
                var s = sender as ListView;
                // if (AppView.PointsGroup.Count > 0)
                // {
                //     var color = AppView.SelectedInTableGroup.Color;
                //     ColorPicker.SelectedColor = color;
                // }
                if (!s.IsMouseOver) return;

                PointsApp.InputControl.OnCurrentGroupChanged(selectedItem.Index);
            }
        }

        private void ClrPicker_Background_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var s = (ColorPicker)sender;
            
            var newColor = s.SelectedColor;
            if (newColor is null) { return; }

            if (e.OldValue is not null)
            {
                //PointsApp.PointContext.SelectNewColor(newColor.Value);
                //PointsApp.ForceRender();
            }
        }

        private void ColorPicker_OnOpened(object sender, RoutedEventArgs e)
        {
            if (AppView.CurrentGroupIndex >= 0) return;
            
            e.Handled = true;
            ColorPicker.IsOpen = false;
        }

        private void ColorPicker_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void ColorPicker_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ColorPicker.SelectedColor != AppView.SelectedInTableGroup.Color && ColorPicker.SelectedColor is not null)
            {
                //PointsApp.PointContext.SelectNewColor(ColorPicker.SelectedColor.Value);
                PointsApp.PushAction(new ChangeColorAction(PointsApp, ColorPicker.SelectedColor.Value));
                PointsApp.ForceRender();
            }
        }
    }
}
