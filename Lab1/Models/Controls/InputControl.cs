using System;
using System.Windows.Input;
using SharpGL.WPF;

namespace Lab1.Models.Controls;

public class InputControl : IInputControlState
{
    private readonly PointsApp _app;
    private IInputControlState _state;

    public InputControl(PointsApp app)
    {
        _app = app;
        _state = new InitialState(app);
    }

    public IInputControlState OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        _state = _state.OnMouseHover(glControl, e);
        return this;
    }

    public IInputControlState OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        _state = _state.OnMouseLeave(glControl, e);
        return this;
    }

    public IInputControlState OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        _state = _state.OnMouseEnter(glControl, e);
        return this;
    }

    public IInputControlState OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _state = _state.OnLeftClick(glControl, e);
        return this;
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _state = _state.OnRightClick(glControl, e);
        return this;
    }

    public IInputControlState OnUndoButton()
    {
        _state = _state.OnUndoButton();
        return this;
    }

    public void ForceChangeState(AppState state)
    {
        _state = state switch
        {
            AppState.Initial => new InitialState(_app),
            AppState.PointPlacement => new PointPlacementState(_app),
            AppState.PointEditing => throw new NotImplementedException(),
            AppState.SelectingPointToEdit => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}