using System.Windows.Input;
using SharpGL.WPF;

namespace Lab1.Models.Controls;

public class InputControl : IInputControlState
{
    private readonly PointContext _pointContext;
    private IInputControlState _state;

    public InputControl(PointContext pointContext)
    {
        _pointContext = pointContext;
        _state = new InitialState(_pointContext);
    }

    public IInputControlState OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public IInputControlState OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public IInputControlState OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public IInputControlState OnClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}