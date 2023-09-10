using System.Windows.Input;
using SharpGL.WPF;

namespace Lab1.Models.Controls;

public class InputControl : IInputControlState
{
    private IInputControlState _state;

    public InputControl(PointsApp app)
    {
        _state = new InitialState(app.PointContext, app);
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

    public IInputControlState OnClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _state = _state.OnClick(glControl, e);
        return this;
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _state = _state.OnRightClick(glControl, e);
        return this;
    }
}