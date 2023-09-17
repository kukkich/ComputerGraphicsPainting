using System;
using System.Drawing;
using System.Windows.Input;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1.Models.Controls;

public abstract class BaseState : IInputControlState
{
    protected PointsApp App { get; }
    protected PointContext PointContext => App.PointContext;

    protected BaseState(PointsApp app)
    {
        App = app;
    }

    protected PointF GetCursorPosition(OpenGLControl glControl, MouseEventArgs e)
    {
        var position = e.GetPosition(glControl);
        return new PointF(
            (float) (position.X / glControl.ActualWidth) * 2 - 1,
            -((float) (position.Y / glControl.ActualHeight) * 2 - 1)
        );
    }

    public virtual void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        var cursorPosition = GetCursorPosition(glControl, e);

        PointContext.Cursor.Position = cursorPosition;
    }

    public virtual void OnMouseLeave(OpenGLControl glControl, MouseEventArgs e)
    {
        PointContext.Cursor.InCanvas = false;
    }

    public virtual void OnMouseEnter(OpenGLControl glControl, MouseEventArgs e)
    {
        PointContext.Cursor.InCanvas = true;
    }

    public virtual void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e) { }

    public virtual void OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e) { }

    public virtual void OnUndoButton()
    {
        App.UndoAction();
    }

    public virtual void OnRedoButton()
    {
        App.RedoAction();
    }

    public virtual void OnEditModeToggle()
    {
        // if (App.PointContext.CurrentGroup is null)
        if (App.State is not AppState.PointPlacement)
        {
            throw new InvalidOperationException("Группа не выбрана");
        }
        App.PushAction(new ChangeStateAction(App, AppState.SelectingPointToEdit));
    }

    public void OnCurrentGroupChanged(int newGroupIndex)
    {
        if (App.State is not (AppState.Initial or AppState.PointPlacement))
        {
            throw new InvalidOperationException("Закончите редактирование точки");
        }

        App.PushAction(new SelectGroupAction(App, newGroupIndex));
        App.ForceRender();
    }
}