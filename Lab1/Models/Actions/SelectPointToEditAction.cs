namespace Lab1.Models.Actions;

public class SelectPointToEditAction : PreviousStateMemorizedAction
{
    private readonly int _pointIndex;
    private PointContext PointsContext => App.PointContext;

    public SelectPointToEditAction(PointsApp app, int pointIndex) 
        : base(app)
    {
        _pointIndex = pointIndex;
    }

    public override void Do()
    {
        base.Do();

        App.State = AppState.PointEditing;
        PointsContext.StartPointEditing(_pointIndex);
    }

    public override void Undo()
    {
        base.Undo();

        PointsContext.StopPointEditing();
    }
}