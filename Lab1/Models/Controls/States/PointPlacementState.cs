using System.Windows.Input;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1.Models.Controls.States;

public class PointPlacementState : BaseState
{

    public PointPlacementState(PointsApp app) 
        : base(app)
        { }

    public override void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        base.OnMouseHover(glControl, e);
        App.ForceRender();
    }

    public override void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        App.PushAction(new AddPointAction(
            PointContext,
            GetCursorPosition(glControl, e)
        ));
    }

    public override void OnRightClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        App.PushAction(new ChangeStateAction(App, AppState.Initial));
    }
}