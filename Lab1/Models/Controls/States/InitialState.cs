using SharpGL.WPF;
using System.Windows.Input;
using Lab1.Models.Actions;

namespace Lab1.Models.Controls.States;

public class InitialState : BaseState
{
    public InitialState(PointsApp app)
        : base(app)
    { }

    public override void OnLeftClick(OpenGLControl glControl, MouseButtonEventArgs e)
    {
        App.PushAction(new AddGroupAction(App));

        App.PushAction(new AddPointAction(
            PointContext,
            GetCursorPosition(glControl, e)
        ));
    }
}