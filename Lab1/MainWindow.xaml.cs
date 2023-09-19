using Lab1.Models;
using Lab1.Models.Actions;
using Lab1.Models.Controls;
using Lab1.ViewModels;
using SharpGL.WPF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace Lab1
{
    public partial class MainWindow : Window
    {
        public PointsApp PointsApp { get; }
        public PointsAppView AppView { get; }
        public InputControl InputControl => PointsApp.InputControl;

        public MainWindow()
        {
            InitializeComponent();

            AppView = new PointsAppView(GroupsTable, this);
            PointsApp = new PointsApp(Dispatcher, AppView);


            DataContext = this;
        }


        public void ShowGroupActions()
        {
            var color = AppView.SelectedInTableGroup.Color;
            ColorPicker.SelectedColor = color;
            ColorPicker.ShowDropDownButton = true;

            RemoveButton.IsEnabled = true;
            ChangeEditModeButton.IsEnabled = true;
            StopEditButton.IsEnabled = true;
        }

        public void HideGroupActions()
        {
            RemoveButton.IsEnabled = false;
            ChangeEditModeButton.IsEnabled = false;
            StopEditButton.IsEnabled = false;

            ColorPicker.SelectedColor = null;
            ColorPicker.ShowDropDownButton = false;
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

                if (!s.IsMouseOver) return;

                PointsApp.InputControl.OnCurrentGroupChanged(selectedItem.Index);
            }
        }

        private void ClrPicker_Background_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
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
                PointsApp.PushAction(new ChangeColorAction(PointsApp, ColorPicker.SelectedColor.Value));
                PointsApp.ForceRender();
            }
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PointsApp.State is AppState.PointPlacement)
            {
                PointsApp.PushAction(new RemoveGroupAction(PointsApp, PointsApp.PointContext.CurrentGroupIndex));
            }
        }

        private void UndoButton_OnClick(object sender, RoutedEventArgs e)
        {
            PointsApp.InputControl.OnUndoButton();
        }

        private void RedoButton_OnClick(object sender, RoutedEventArgs e)
        {
            PointsApp.InputControl.OnRedoButton();
        }

        private void ChangeEditModeButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PointsApp.InputControl.OnEditModeToggle();
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void StopEditButton_OnClick(object sender, RoutedEventArgs e)
        {
            PointsApp.PushAction(new ChangeStateAction(PointsApp, AppState.Initial));
        }
    }
}
