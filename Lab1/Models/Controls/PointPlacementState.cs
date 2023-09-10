using System;
using System.Drawing;
using System.Threading;
using System.Windows.Input;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1.Models.Controls;

public class PointPlacementState : IInputControlState
{
    private readonly PointContext _pointContext;
    private readonly PointsApp _app;

    public PointPlacementState(PointContext pointContext, PointsApp app)
    {
        _pointContext = pointContext;
        _app = app;
    }

    public IInputControlState OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);

        var cursorPosition = new PointF(
             (float)(position.X / glControl.ActualWidth) * 2 - 1,
            -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        );

        _pointContext.CursorPosition = cursorPosition;
        _app.RenderScheduled = true;
        return this;
    }

    public IInputControlState OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        _pointContext.CursorPosition = null;
        return this;
    }

    public IInputControlState OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);

        var cursorPosition = new PointF(
            (float)(position.X / glControl.ActualWidth) * 2 - 1,
            -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        );

        _pointContext.CursorPosition = cursorPosition;
        return this;
    }

    public IInputControlState OnClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        var position = e.GetPosition(glControl);

        _app.PushAction(new AddPointAction(
            _pointContext,
            new PointF(
                (float)(position.X / glControl.ActualWidth) * 2 - 1,
                -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        )));
        return this;
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _pointContext.CursorPosition = null;

        return new InitialState(_pointContext, _app);
    }
}