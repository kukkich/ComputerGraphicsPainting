using System.Drawing;
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

    public void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);

        var cursorPosition = new PointF(
             (float)(position.X / glControl.ActualWidth) * 2 - 1,
            -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        );

        PointContext.Cursor.Position = cursorPosition;
        _app.RenderScheduled = true;
    }

    public void OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        PointContext.Cursor.InCanvas = false;
    }

    public void OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);

        var cursorPosition = new PointF(
            (float)(position.X / glControl.ActualWidth) * 2 - 1,
            -((float)(position.Y / glControl.ActualHeight) * 2 - 1)
        );

        PointContext.Cursor.InCanvas = true;
        PointContext.Cursor.Position = cursorPosition;
    }

    public void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
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
        _app.PushAction(new ChangeStateAction(_app, AppState.Initial));
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