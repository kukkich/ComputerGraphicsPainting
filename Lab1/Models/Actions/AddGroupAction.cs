namespace Lab1.Models.Actions;

public class AddGroupAction : OneTimeAction
{
    private readonly PointContext _pointsContext;

    public AddGroupAction(PointContext pointsContext)
    {
        _pointsContext = pointsContext;
    }

    public override void Do()
    {
        base.Do();
        _pointsContext.AddGroup();
    }

    public override void Undo()
    {
        base.Undo();
        _pointsContext.RemoveLastGroup();
    }
}