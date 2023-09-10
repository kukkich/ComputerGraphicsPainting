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
            var id = Thread.CurrentThread.ManagedThreadId;
            InitializeComponent();
            PointsApp = new PointsApp(Dispatcher);
        }

        private void OpenGLControl_OpenGLInitialized(object s, OpenGLRoutedEventArgs args)
        {
            args.OpenGL.ClearColor(0.5f, 0.3f, 0.3f, 0.3f);
            // PointsApp.InitOpenGL((OpenGLControl)s, args);
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            // var points = PointContext.CurrentGroup;
            // var id = Thread.CurrentThread.ManagedThreadId;
            // var gl = args.OpenGL;
            // gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            // gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
            // gl.Color(PointsApp.Color);
            // gl.ClearColor(Random.Shared.NextSingle(), Random.Shared.NextSingle(), Random.Shared.NextSingle(), 0.3f);
            //
            // foreach (var p in points)
            // {
            //     gl.Vertex(p.X, p.Y);
            // }
            //
            // if (PointContext.CursorPosition is not null)
            // {
            //     gl.Vertex(
            //         PointContext.CursorPosition.Value.X,
            //         PointContext.CursorPosition.Value.Y
            //     );
            // }
            //
            // gl.End();

            var points = 
            new[,]
            {
                {-0.5f, -1f},
                {-1f, 0f},
                {-0.5f, 0f},
                {0f, 0.5f},
                {0f, 1f},
                // {-0.4f, 0f},
            
            };
            var gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl.Color(new []{1, 1, 1, 1f});

            for (int i = 0; i < points.GetLength(0); i++)
            {
                gl.Vertex(points[i, 0], points[i, 1]);
            }

            gl.End();
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
            InputControl.OnClick((OpenGLControl)sender, e);
        }

        private void UIElement_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InputControl.OnMouseEnter((OpenGLControl)sender, e);

        }
    }
}
