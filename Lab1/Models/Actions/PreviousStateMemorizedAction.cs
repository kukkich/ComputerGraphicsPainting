using System;

namespace Lab1.Models.Actions;

public abstract class PreviousStateMemorizedAction : IAction
{
    protected PointsApp App { get; }
    private AppState _oldState;

    protected PreviousStateMemorizedAction(PointsApp app)
    {
        App = app;
    }

    public virtual void Do()
    {
        _oldState = App.State;
    }

    public virtual void Undo()
    {
        App.State = _oldState;
    }
}