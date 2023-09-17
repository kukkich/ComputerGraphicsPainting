using System;

namespace Lab1.Models.Actions;

public class ChangeStateAction : IAction
{
    private readonly PointsApp _app;
    private readonly AppState _newState;
    private AppState? _oldState;
    
    public ChangeStateAction(PointsApp app, AppState newState)
    {
        _app = app;
        _newState = newState;
    }

    public void Do()
    {
        _oldState = _app.State;
        _app.State = _newState;
    }

    public void Undo()
    {
        if (_oldState is null)
        {
            throw new InvalidOperationException();
        }

        _app.State = _oldState.Value;
    }
}