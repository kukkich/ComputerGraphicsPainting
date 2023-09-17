using System.Windows.Input;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1.Models.Controls.States;

public class PointEditingState : BaseState
{
    public PointEditingState(PointsApp app) 
        : base(app)
        { }

    public override void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        base.OnMouseHover(glControl, e);
        App.ForceRender();
    }

    public override void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        base.OnLeftClick(glControl, e);
        App.PushAction(new CommitPointEditingAction(App, App.PointContext.Cursor.Position));
    }

    public override void OnEditModeToggle() { }
}