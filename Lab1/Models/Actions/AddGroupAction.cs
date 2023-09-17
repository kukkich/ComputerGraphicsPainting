namespace Lab1.Models.Actions;

public class AddGroupAction : PreviousStateMemorizedAction
{
    private PointContext PointsContext => App.PointContext;

    public AddGroupAction(PointsApp app) 
        : base(app)
        { }

    public override void Do()
    {
        base.Do();
        
        App.State = AppState.PointPlacement;
        PointsContext.AddGroup();
    }

    public override void Undo()
    {
        base.Undo();
        
        PointsContext.RemoveLastGroup();
    }
}