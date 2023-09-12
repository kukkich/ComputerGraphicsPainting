namespace Lab1.Models.Actions;

public class AddGroupAction : OneTimeAction
{
    private readonly PointsApp _app;
    private PointContext PointsContext => _app.PointContext;
    private AppState _oldState;

    public AddGroupAction(PointsApp app)
    {
        _app = app;
    }

    public override void Do()
    {
        base.Do();

        _oldState = _app.State;
        _app.State = AppState.PointPlacement;

        PointsContext.AddGroup();
    }

    public override void Undo()
    {
        base.Undo();

        _app.State = _oldState;

        PointsContext.RemoveLastGroup();
    }
}