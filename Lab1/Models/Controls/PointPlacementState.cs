using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1.Models.Controls;

public class PointPlacementState : IInputControlState
{
    private PointContext PointContext => _app.PointContext;
    private readonly PointsApp _app;
    public PointPlacementState(PointsApp app)
    {
        _app = app;
    }

    public IInputControlState OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);

        var cursorPosition = new PointF(
             (float)(position.X / glControl.ActualWidth) * 2 - 1,
            -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        );

        PointContext.Cursor.Position = cursorPosition;
        _app.RenderScheduled = true;
        return this;
    }

    public IInputControlState OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        // _app.PushAction(new ChangeStateAction(_app, AppState.Initial));
        PointContext.Cursor.InCanvas = false;
        return this;
    }

    public IInputControlState OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);

        var cursorPosition = new PointF(
            (float)(position.X / glControl.ActualWidth) * 2 - 1,
            -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        );

        // _app.PushAction(new ChangeStateAction(_app, AppState.PointPlacement));

        PointContext.Cursor.InCanvas = true;
        PointContext.Cursor.Position = cursorPosition;
        return this;
    }

    public IInputControlState OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        // _app.State = AppState.PointPlacement;
        var position = e.GetPosition(glControl);

        _app.PushAction(new AddPointAction(
            PointContext,
            new PointF(
                (float)(position.X / glControl.ActualWidth) * 2 - 1,
                -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        )));

        return this;
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        // _app.PushAction(new AddGroupAction(_app));
        _app.PushAction(new ChangeStateAction(_app, AppState.Initial));
        return new InitialState(_app);
    }

    public IInputControlState OnUndoButton()
    {
        _app.UndoAction();

        return this;
    }
}