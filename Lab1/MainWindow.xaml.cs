using System;
using System.Drawing;
using System.Threading;
using SharpGL;
using System.Windows;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1
{
    public partial class MainWindow : Window
    {
        private byte[] _color;
        private OpenGLControl _glControl;
        private float? mouseX;
        private float? mouseY;
        private Timer renderTimer;
        private bool renderScheduled = false;

        private readonly PointsApp pointsApp;
        private readonly PointContext pointContext;

        public MainWindow()
        {
            InitializeComponent();
            pointsApp = new PointsApp();
            pointContext = pointsApp.PointContext;
            _color = new byte[] { 0, 1, 0 };
            renderTimer = new Timer(RenderTimerCallback, null, 0, 20);
        }

        private void OpenGLControl_OpenGLInitialized(object s, OpenGLRoutedEventArgs args)
        {
            if (s is not OpenGLControl sender)
            {
                return;
            }
            
            _glControl = sender;
            sender.RenderTrigger = RenderTrigger.TimerBased;
            var gl = args.OpenGL;
            gl.ClearColor(0.3f, 0.3f, 0.3f, 0.3f);
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            var points = pointContext.CurrentGroup;
            // new[,]
            // {
            //     {-0.5f, -1f},
            //     {-1f, 0f},
            //     {-0.5f, 0f},
            //     {0f, 0.5f},
            //     {0f, 1f},
            //     // {-0.4f, 0f},
            //
            // };
            var gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl.Color(_color);
            
            foreach (var p in points)
            {
                gl.Vertex(p.X, p.Y);
            }

            if (mouseX != null && mouseY != null)
            {
                gl.Vertex((float)mouseX, (float)mouseY);
            }

            gl.End();
        }

        private void OpenGLControl_Resized(object sender, OpenGLRoutedEventArgs args)
        {
        }

        private void UIElement_OnMouseDown(object s, MouseButtonEventArgs e)
        {
            Random.Shared.NextBytes(_color);

            pointsApp.PushAction(new AddPointAction(
                pointContext,
                new PointF((float)mouseX!, (float)mouseY!))
            );
            _glControl.DoRender();
        }
        private void RenderTimerCallback(object state)
        {
            if (!renderScheduled) return;

            Dispatcher.Invoke(_glControl.DoRender);
            renderScheduled = false;
        }

        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(_glControl);

            // Преобразуем координаты в диапазон OpenGL (-1, 1)
            mouseX = (float)(position.X / _glControl.ActualWidth) * 2 - 1;
            mouseY = -((float)(position.Y / _glControl.ActualHeight) * 2 - 1);
                
            // Вызываем перерисовку OpenGLControl
            renderScheduled = true;
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            mouseX = null;
            mouseY = null;
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void UIElement_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
