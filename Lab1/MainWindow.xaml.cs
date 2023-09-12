using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Models.Controls;
using SharpGL;
using SharpGL.WPF;

namespace Lab1
{
    public partial class MainWindow : Window
    {
        public PointsApp PointsApp { get; }
        public PointContext PointContext => PointsApp.PointContext;
        public InputControl InputControl => PointsApp.InputControl;

        public MainWindow()
        {
            InitializeComponent();
            PointsApp = new PointsApp(Dispatcher);
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
            InputControl.OnMouseHover((OpenGLControl) sender, e);
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
                // MessageBox.Show("нажали ctrl + Z");
                PointsApp.InputControl.OnUndoButton();
            }
        }
    }
}
