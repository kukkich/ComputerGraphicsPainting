using SharpGL.WPF;
using System.Windows.Input;
using Lab1.Models.Actions;

namespace Lab1.Models.Controls;

public class InitialState : IInputControlState
{
    private readonly PointContext _pointContext;
    private readonly PointsApp _app;

    public InitialState(PointContext pointContext, PointsApp app)
    {
        _pointContext = pointContext;
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

    public IInputControlState OnClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _app.PushAction(new AddGroupAction(_pointContext));

        var newState = new PointPlacementState(_pointContext, _app);

        return newState.OnClick(glControl, e);
    }

    public IInputControlState OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        return this;
    }
}