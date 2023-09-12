using System;
using System.Windows;
using SharpGL.WPF;
using System.Windows.Input;
using Lab1.Models.Actions;

namespace Lab1.Models.Controls;

public class InitialState : IInputControlState
{
    private PointContext PointContext => _app.PointContext;
    private readonly PointsApp _app;

    public InitialState(PointsApp app)
    {
        _app = app;
    }

    public IInputControlState OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        return this;
    }

    public IInputControlState OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        return this;
    }

    public IInputControlState OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        return this;
    }

    public IInputControlState OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _app.PushAction(new AddGroupAction(_app));

        var newState = new PointPlacementState(_app);

        return newState.OnLeftClick(glControl, e);
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        return this;
    }

    public IInputControlState OnUndoButton()
    {
        _app.UndoAction();
        return this;
    }
}