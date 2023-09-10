using SharpGL.WPF;
using System.Windows.Input;

namespace Lab1.Models.Controls;

public class InitialState : IInputControlState
{
    private readonly PointContext _pointContext;

    public InitialState(PointContext pointContext)
    {
        _pointContext = pointContext;
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

    public IInputControlState OnClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        var newState = new PointPlacementState(_pointContext);
        return newState.OnClick(glControl, e);
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        return this;
    }
}