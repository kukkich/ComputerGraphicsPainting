using System.Windows.Input;
using Lab1.Models.Actions;
using SharpGL.WPF;

namespace Lab1.Models.Controls.States;

public class SelectingPointToEditState : BaseState
{
    public SelectingPointToEditState(PointsApp app) 
        : base(app)
        { }

    public override void OnMouseHover(OpenGLControl glControl, MouseEventArgs e)
    {
        base.OnMouseHover(glControl, e);
        PointContext.SelectClosestPointInCurrentGroup();
        App.ForceRender();
    }

    public override void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        base.OnLeftClick(glControl, e);
        if (PointContext.ClosestPointIndex is not null)
        {
            App.PushAction(new SelectPointToEditAction(App, PointContext.ClosestPointIndex.Value));
        }
    }

    public override void OnEditModeToggle()
    {
        App.PushAction(new ChangeStateAction(App, AppState.PointEditing));
    }
}