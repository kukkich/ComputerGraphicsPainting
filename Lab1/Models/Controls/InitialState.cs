using System.Drawing;
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

    public void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
    }

    public void OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
    }

    public void OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
    }

    public void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        _app.PushAction(new AddGroupAction(_app));

        var position = e.GetPosition(glControl);

        _app.PushAction(new AddPointAction(
            PointContext,
            new PointF(
                (float)(position.X / glControl.ActualWidth) * 2 - 1,
                -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
            )));
    }

    public void OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
    }

    public void OnUndoButton()
    {
        _app.UndoAction();
    }

    public void OnRedoButton()
    {
        _app.RedoAction();
    }
}